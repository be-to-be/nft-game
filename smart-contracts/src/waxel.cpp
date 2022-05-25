#include "ninjas.cpp"
#include <stdint.h>

using namespace eosio;

ACTION waxel::findmat(name account, uint64_t assetID)
{
  require_auth(account);

  auto player = user.find(account.value);
  check(player != user.end(), "Account not registered!");

  auto profess = professions.find(assetID);
  check(profess != professions.end(), "Profession not registered!");

  check(profess->owner == account, "Account does not have this profession");
  check(profess->type == "Gatherer", "Cannot use this profession to mine!");

  check(profess->uses_left > 0, "This profession has no uses left!");
  check(profess->last_material_search.utc_seconds != 0,
        "Start seach before finding material!");
  uint32_t diff = current_time_point_sec().utc_seconds -
                  profess->last_material_search.utc_seconds;

  check(diff > FINDING_DELAY, "Cannot claim result yet!");

  for (int i = 0; i < profess->items.size(); i++)
  {
    auto item = items.find(profess->items[i]);
    check(item != items.end(), "Item does not exist");
    check(item->equipped, "Item has not been equipped");
    check(item->uses_left > 0, "One of the items has no uses left unequip it first");
  }

  auto size = transaction_size();

  char buf[size];

  auto read = read_transaction(buf, size);
  check(size == read, "read_transaction() has failed.");

  checksum256 tx_id = eosio::sha256(buf, read);

  uint64_t signing_value;

  memcpy(&signing_value, tx_id.data(), sizeof(signing_value));

  action(permission_level{get_self(), name("active")}, name("orng.wax"),
         name("requestrand"),
         std::make_tuple(assetID, // used as assoc id
                         signing_value, get_self()))
      .send();

  professions.modify(profess, get_self(),
                     [&](auto &v)
                     { v.status = "holdup"; });
}

ACTION waxel::craftitem(name account, uint64_t professID)
{
  require_auth(account);

  auto player = user.find(account.value);
  check(player != user.end(), "Account not registered!");
  auto config = configs.begin();

  auto profess = professions.find(professID);
  check(profess != professions.end(), "Profession not registered!");
  check(profess->last_material_search.utc_seconds != 0,
        "Start seach before finding material!");
  check(profess->owner == account, "Account does not have this profession");
  check(profess->type == "Refiner and crafter",
        "Cannot use this profession to craft!");
  check(profess->name == "Engineer" || profess->name == "Blacksmith", "This profession Cannot be used for crafting");

  check(profess->uses_left > 0, "No uses left for this profession");
  size_t start;
  size_t end = 0;
  char delim = '%';
  vector<string> out;
  string memo = profess->status;
  while ((start = memo.find_first_not_of(delim, end)) != string::npos)
  {
    end = memo.find(delim, start);
    out.push_back(memo.substr(start, end - start));
  }

  string type = out[0];
  check(type == "Crafting", "Cannot craft with this proffesion yet");
  uint32_t itemID = (uint32_t)stoull(out[1]);

  auto itemcombo = config->item_combo;
  uint32_t delay = 0;
  bool found = false;
  for (int i = 0; i < itemcombo.size(); i++)
  {
    if (itemcombo[i].template_id == itemID)
    {
      found = true;
      delay = itemcombo[i].delay;
    }
  }

  check(found, "Invalid item ID");
  check(delay != 0, "Invalid item ID");
  uint32_t diff = current_time_point_sec().utc_seconds -
                  profess->last_material_search.utc_seconds;

  check(diff >= delay, "Cannot craft yet!");

  action(permission_level{get_self(), name("active")}, name("atomicassets"),
         name("mintasset"),
         make_tuple(get_self(), COLLECTION_NAME, name("items"),
                    (uint32_t)itemID, account, (atomicassets::ATTRIBUTE_MAP){},
                    (atomicassets::ATTRIBUTE_MAP){}, (vector<asset>){}))
      .send();

  uint64_t uses = profess->uses_left;
  uses -= (uint64_t)1;
  atomicassets::ATTRIBUTE_MAP mdata = get_mutabledata(profess->owner, professID);
  mdata["Work (uses left)"] = uses;
  update_assert_on_atomic(profess->owner, profess->asset_id, mdata);

  professions.modify(profess, get_self(), [&](auto &v)
                     {
    v.status = "Crafted%"+out[1];
    v.last_material_search.utc_seconds = 0;
    v.uses_left = uses; });
}

ACTION waxel::mintcitizens(name account, int amount)
{

  require_auth(account);
  auto player = user.find(account.value);
  check(player != user.end(), "No user found");
  auto config = configs.begin();

  int citizens = player->citizen_count;
  check(citizens >= amount * 10, "Not enough cz balance");
  int id = finder(config->template_ids, "Citizen10X");
  uint32_t template_id = (uint32_t)config->template_ids[id].value;
  int final = amount;
  while (amount != 0)
  {

    action(permission_level{get_self(), name("active")}, name("atomicassets"),
           name("mintasset"),
           make_tuple(get_self(), COLLECTION_NAME, name("citizens"),
                      (uint32_t)template_id, account,
                      (atomicassets::ATTRIBUTE_MAP){},
                      (atomicassets::ATTRIBUTE_MAP){}, (vector<asset>){}))
        .send();
    amount--;
    citizens = citizens - 10;
  }

  addczbalance(account, -final * 10);
}

ACTION waxel::unequipitems(vector<uint64_t> item_ids)
{

  for (int j = 0; j < item_ids.size(); j++)
  {
    auto item = items.find(item_ids[j]);
    name owner = item->owner;
    auto profess = professions.find(item->profession);

    check(profess != professions.end(), "Profession is not registered");

    vector<uint64_t> item_inventory = profess->items;

    check(has_auth(owner) || has_auth(get_self()), "No authority!");
    auto player = user.find(owner.value);
    check(player != user.end(), "User is not registered");

    check(item != items.end(), "No item found!");
    check(item->equipped == true, "Item is not equipped!");

    for (int i = 0; i < item_inventory.size(); i++)
    {
      if (item_inventory[i] == item_ids[j])
      {
        item_inventory.erase(item_inventory.begin() + i);
      }
    }

    item = items.erase(item);
    professions.modify(profess, get_self(), [&](auto &v)
                       {
      v.items = item_inventory;
      v.status = "Idle";
      v.last_material_search.utc_seconds = 0; });
  }
}

ACTION waxel::mintmats(name account, string mat, int amount)
{

  require_auth(account);
  auto player = user.find(account.value);
  check(player != user.end(), "No user found");
  auto config = configs.begin();

  int id = finder(config->template_ids, mat);
  uint32_t template_id = (uint32_t)config->template_ids[id].value;
  check(amount <= 10 && amount > 0, "Exceeded limit of minting materials");
  symbol mat_s = symbol(symbol_code(mat), 2);
  int mat_amount = amount * 10 * 100;
  addmtbalance(account, mat_amount, mat_s, false);

  while (amount != 0)
  {

    action(permission_level{get_self(), name("active")}, name("atomicassets"),
           name("mintasset"),
           make_tuple(get_self(), COLLECTION_NAME, name("materials"),
                      (uint32_t)template_id, account,
                      (atomicassets::ATTRIBUTE_MAP){},
                      (atomicassets::ATTRIBUTE_MAP){}, (vector<asset>){}))
        .send();
    amount--;
  }
}

ACTION waxel::refinemat(name account, uint64_t professID)
{
  require_auth(account);

  auto profess = professions.find(professID);
  check(profess != professions.end(), "No profession found!");

  check(profess->type != "Gatherer", "Cannot use this profession to refine!");
  check(profess->uses_left > 0, "No uses left for this profession");
  check(profess->last_material_search.utc_seconds != 0,
        "Start seach before finding material!");
  check(profess->name == "Carpenter" || profess->name == "Blacksmith" || profess->name == "Tailor", "This profession Cannot be used for refining");

  uint32_t delay = current_time_point_sec().utc_seconds -
                   profess->last_material_search.utc_seconds;

  check(delay >= REFINING_DELAY, "Cannot refine yet!");
  size_t start;
  size_t end = 0;
  char delim = '%';
  vector<string> out;
  string memo = profess->status;
  while ((start = memo.find_first_not_of(delim, end)) != string::npos)
  {
    end = memo.find(delim, start);
    out.push_back(memo.substr(start, end - start));
  }

  string type = out[0];
  check(type == "Refining", "Cannot craft with this proffesion yet");
  string mat = out[1];

  uint64_t uses = profess->uses_left;
  uses -= (uint64_t)1;
  atomicassets::ATTRIBUTE_MAP mdata = get_mutabledata(profess->owner, professID);
  mdata["Work (uses left)"] = uses;
  update_assert_on_atomic(profess->owner, profess->asset_id, mdata);

  professions.modify(profess, get_self(), [&](auto &v)
                     {
    v.status = "Refined%"+mat;
    v.last_material_search.utc_seconds = 0;
    v.uses_left =uses; });
}

ACTION waxel::equipitems(uint64_t profession_id, vector<uint64_t> item_ids)
{
  auto profess = professions.find(profession_id);

  check(profess != professions.end(), "Profession is not registered");
  name owner = profess->owner;

  check(has_auth(owner) || has_auth(get_self()), "No authority!");

  name ram_payer = has_auth(get_self()) ? get_self() : owner;

  auto player = user.find(profess->owner.value);

  check(player != user.end(), "User is not registered");

  check(profess->type == "Gatherer", "Cannot equip item to this profession!");
  auto data = profess->items;
  atomicassets::assets_t own_assets = atomicassets::get_assets(profess->owner);
  auto config = configs.begin();
  for (int j = 0; j < item_ids.size(); j++)
  {
    check(data.size() < 3, "Max items registered");
    auto item = items.find(item_ids[j]);
    uint64_t asset_id = item_ids[j];
    auto asset_itr =
        own_assets.require_find(item_ids[j], "The player does not own this asset");

    atomicassets::schemas_t collection_schemas =
        atomicassets::get_schemas(asset_itr->collection_name);
    auto schema_itr = collection_schemas.find(asset_itr->schema_name.value);

    atomicassets::templates_t collection_templates =
        atomicassets::get_templates(asset_itr->collection_name);
    auto template_itr = collection_templates.find(asset_itr->template_id);

    vector<uint8_t> immutable_serialized_data =
        template_itr->immutable_serialized_data;

    atomicassets::ATTRIBUTE_MAP idata =
        atomicdata::deserialize(immutable_serialized_data, schema_itr->format);

    vector<uint8_t> mutable_serialized_data =
        asset_itr->mutable_serialized_data;

    atomicassets::ATTRIBUTE_MAP mdata =
        atomicdata::deserialize(mutable_serialized_data, schema_itr->format);
    if (item == items.end())
    {

      string type = get<string>(idata["Type"]);
      string name = get<string>(idata["name"]);
      string rarity = get<string>(idata["Rarity"]);

      uint64_t uses = 60;
      uint64_t last_used = 0;

      time_point_sec last_time = time_point_sec(last_used);

      auto checkd = mdata.find("Durability (uses left)");
      if (checkd != mdata.end())
        uint64_t uses = get<uint64_t>(mdata["Durability (uses left)"]);
      else
      {
        mdata["Durability (uses left)"] = uses;
        update_assert_on_atomic(owner, asset_id, mdata);
      }
      pair_string_float32 temp;
      temp.value = rarity == "Common" ? 5 : rarity == "Uncommon" ? 10
                                                                 : 15;
      uint32_t t_id = asset_itr->template_id;
      auto itemcombo = config->item_combo;
      string ty = "";
      for (auto var : itemcombo)
      {
        if (var.template_id == t_id)
        {
          ty = var.type;
        }
      }
      temp.key = ty;
      items.emplace(ram_payer, [&](auto &v)
                    {
          v.asset_id = asset_id;
          v.owner = owner;
          v.name = name;
          v.function = temp;
          v.profession=profession_id;
          v.equipped = true;
          v.last_material_search = last_time;
          v.uses_left = uses;
          v.status = "Idle"; });
    }
    else
    {
      check(item->equipped == false, "Item is already equipped!");

      auto itemz = items.find(item_ids[j]);
      check(itemz != items.end(), "Error found");
      items.modify(itemz, get_self(), [&](auto &v)
                   {
      v.equipped = true;
      v.profession = profession_id; });
    }
    for (auto var : data)
    {
      auto item2 = items.find(var);
      auto function = item2->function;
      check(function.key != item->function.key,
            "Cannot equip same function item again");
    }
    data.push_back(item_ids[j]);
  }

  professions.modify(profess, get_self(), [&](auto &v)
                     { v.items = data; });
}

void waxel::receive_asset_transfer(name from, name to,
                                   vector<uint64_t> asset_ids, string memo)
{

  if (to != get_self() || from == get_self())
  {
    return;
  }
  if (memo == "blendprofession")
  {
    blendprofess(from, to, asset_ids);
  }
  else if (memo == "regupgrade")
  {
    regupgrade(from, to, asset_ids);
  }
  else
    check(false, "Invalid memo!");
}

void waxel::regupgrade(name from, name to, vector<uint64_t> asset_ids)
{
  auto config = configs.begin();

  auto player = user.find(from.value);
  if (player == user.end())
    regnewplayer(from, get_self());
  player = user.find(from.value);

  check(asset_ids.size() < 2, "Can only register 1 upgrade at a time");

  atomicassets::assets_t own_assets = atomicassets::get_assets(get_self());
  auto asset_itr = own_assets.find(asset_ids[0]);

  check(asset_itr->template_id != -1,
        "The transferred asset does not belong to a template");

  name schema_name = asset_itr->schema_name;
  check(schema_name == UPGRADE_SCHEMA,
        "Provided asset ID" + to_string(asset_ids[0]) + "is invalid");

  atomicassets::schemas_t collection_schemas =
      atomicassets::get_schemas(asset_itr->collection_name);
  auto schema_itr = collection_schemas.find(asset_itr->schema_name.value);

  atomicassets::templates_t collection_templates =
      atomicassets::get_templates(asset_itr->collection_name);
  auto template_itr = collection_templates.find(asset_itr->template_id);

  vector<uint8_t> immutable_serialized_data =
      template_itr->immutable_serialized_data;

  atomicassets::ATTRIBUTE_MAP idata =
      atomicdata::deserialize(immutable_serialized_data, schema_itr->format);

  atomicassets::ATTRIBUTE_MAP mdata = {};
  string s_name = get<string>(idata["name"]);

  auto settlement = settlements.find(asset_ids[0]);
  check(settlement == settlements.end(), "Already registered");

  settlements.emplace(get_self(), [&](auto &v)
                      {
    v.asset_id = asset_ids[0];
    v.owner = from;
    v.type = s_name; });

  auto nft_counts = player->nft_counts;
  string a_nft = "";
  if (s_name == "Camp")
    a_nft = "Ninja";
  if (s_name == "Mine")
    a_nft = "Miner";
  if (s_name == "Forest")
    a_nft = "Lumberjack";
  if (s_name == "Field")
    a_nft = "Farmer";
  if (a_nft != "Ninja")
  {
    auto maxProfessions = nft_counts.maxProfessions;

    for (int i = 0; i < nft_counts.maxProfessions.size(); i++)
    {
      if (nft_counts.maxProfessions[i].key == a_nft)
      {
        nft_counts.maxProfessions[i].value += 10;
      }
    }
  }
  else
  {
    nft_counts.maxNinja += 10;
  }

  user.modify(player, get_self(), [&](auto &v)
              { v.nft_counts = nft_counts; });
}

ACTION waxel::withdrawnfts(name account, vector<uint64_t> asset_ids)
{
  check(has_auth(account) || has_auth(get_self()), "Not authorized!");
  auto player = user.find(account.value);
  check(player != user.end(), "Not registered user");
  auto nft_counts = player->nft_counts;

  for (int i = 0; i < asset_ids.size(); i++)
  {
    auto settlement = settlements.find(asset_ids[i]);
    check(settlement != settlements.end(), "No settlement found with this ID");
    check(settlement->owner == account, "User does not own this asset ID");

    string s_name = settlement->type;
    string a_nft = "";
    if (s_name == "Camp")
      a_nft = "Ninja";
    if (s_name == "Mine")
      a_nft = "Miner";
    if (s_name == "Forest")
      a_nft = "Lumberjack";
    if (s_name == "Field")
      a_nft = "Farmer";
    if (a_nft != "Ninja")
    {
      uint64_t maxvalue = 0;
      for (auto var : player->nft_counts.maxProfessions)
      {
        if (var.key == a_nft)
        {
          maxvalue = var.value;
        }
      }

      auto maxProfessions = nft_counts.maxProfessions;
      for (int i = 0; i < nft_counts.maxProfessions.size(); i++)
      {
        if (nft_counts.maxProfessions[i].key == a_nft)
        {
          maxvalue = nft_counts.maxProfessions[i].value - 10;
          nft_counts.maxProfessions[i].value = maxvalue;
        }
      }

      check(getnftcount(account, "profession", a_nft) <= maxvalue,
            "Cannot withdraw settlement, unregister respective nfts first");
    }
    else
      check(getnftcount(account, "ninja", "none") <=
                player->nft_counts.maxNinja - 10,
            "Cannot withdraw settlement, unregister respective nfts first");
    nft_counts.maxNinja -= 10;
    if (nft_counts.maxNinja == 0)
      nft_counts.maxNinja = 10;
    settlement = settlements.erase(settlement);
  }

  action(permission_level{get_self(), name("active")}, name("atomicassets"),
         name("transfer"),
         make_tuple(get_self(), account, asset_ids,
                    "Withdrawn settlements from waxelworld " +
                        to_string(asset_ids.size())))
      .send();
  user.modify(player, get_self(), [&](auto &v)
              { v.nft_counts = nft_counts; });
}

ACTION waxel::resetuser(name account)
{
  require_auth(get_self());
  auto player = user.find(account.value);
  check(player != user.end(), "asad");
  auto nft_counts = player->nft_counts;
  if (account == name("slicksheep12"))
    nft_counts.maxNinja = 20;
  else
    nft_counts.maxNinja = 10;
  nft_counts.maxProfessions[0].value = 10;
  nft_counts.maxProfessions[1].value = 20;
  nft_counts.maxProfessions[2].value = 10;

  user.modify(player, get_self(), [&](auto &v)
              { v.nft_counts = nft_counts; });
}

void waxel::blendprofess(name from, name to, vector<uint64_t> asset_ids)
{

  auto config = configs.begin();
  uint64_t citizens_amount = 0;

  check(asset_ids.size() < 2, "Can only craft 1 profession at a time");

  atomicassets::assets_t own_assets = atomicassets::get_assets(get_self());
  auto asset_itr = own_assets.find(asset_ids[0]);

  check(asset_itr->template_id != -1,
        "The transferred asset does not belong to a template");

  name schema_name = asset_itr->schema_name;
  check(schema_name == BOOKS_SCHEMA,
        "Provided asset ID" + to_string(asset_ids[0]) + "is invalid");

  atomicassets::schemas_t collection_schemas =
      atomicassets::get_schemas(asset_itr->collection_name);
  auto schema_itr = collection_schemas.find(asset_itr->schema_name.value);

  atomicassets::templates_t collection_templates =
      atomicassets::get_templates(asset_itr->collection_name);
  auto template_itr = collection_templates.find(asset_itr->template_id);

  vector<uint8_t> immutable_serialized_data =
      template_itr->immutable_serialized_data;

  atomicassets::ATTRIBUTE_MAP idata =
      atomicdata::deserialize(immutable_serialized_data, schema_itr->format);

  atomicassets::ATTRIBUTE_MAP mdata = {};
  string profess1 = get<string>(idata["Type"]);

  profession_blend combo = filterprofession(asset_itr->template_id);
  string profess = combo.profession;
  string type = combo.type;
  citizens_amount = type == "Gatherer" ? 5 : 10;
  addczbalance(from, -citizens_amount);

  check(profess != "", "No such profession combination found");
  int id = finder(config->template_ids, profess);
  uint32_t template_id = (uint32_t)config->template_ids[id].value;
  mintproffess(from, template_id, idata, mdata);
  burnassets(asset_ids);
}

ACTION waxel::unbindnfts(vector<uint64_t> asset_ids, name owner)
{
  check(has_auth(owner) || has_auth(get_self()), "No authority!");
  name ram_payer = has_auth(get_self()) ? get_self() : owner;

  auto player = user.find(owner.value);
  if (player != user.end())
  {
    for (auto asset_id : asset_ids)
    {

      auto ninja = ninjas.find(asset_id);
      if (ninja != ninjas.end())
      {
        ninja = ninjas.erase(ninja);
      }
      else
      {
        auto profession = professions.find(asset_id);

        if (profession != professions.end())
        {
          profession = professions.erase(profession);

          if (profession->items.size() > 0)
          {
            action(permission_level{get_self(), name("active")}, get_self(),
                   name("unequipitems"), std::make_tuple(profession->items))
                .send();
          }
        }
        else
        {
          auto item = items.find(asset_id);
          if (item != items.end())
          {
            if (item->equipped == true)
            {
              vector<uint64_t> ids;
              ids.push_back(asset_id);
              action(permission_level{get_self(), name("active")}, get_self(),
                     name("unequipitems"), std::make_tuple(ids))
                  .send();
            }
            item = items.erase(item);
          }
        }
      }
    }
  }
}
