#include "utils.cpp"
#include <stdint.h>

ACTION waxel::registernfts(vector<uint64_t> asset_ids, name owner)
{
  check(has_auth(owner) || has_auth(get_self()), "No authority!");

  name ram_payer = has_auth(get_self()) ? get_self() : owner;

  auto player = user.find(owner.value);
  if (player == user.end())
  {
    regnewplayer(owner, owner);
  }
  player = user.find(owner.value);

  auto config = configs.begin();
  atomicassets::assets_t own_assets = atomicassets::get_assets(owner);

  for (auto asset_id : asset_ids)
  {
    auto asset_itr =
        own_assets.require_find(asset_id, "The player does not own this asset");

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

    if (asset_itr->schema_name == name("laxewnefty"))
    {
      auto ninja = ninjas.find(asset_id);

      check(ninja == ninjas.end(), "Already registered");
      {
        check(getnftcount(owner, "ninja", "none") < player->nft_counts.maxNinja, "Max ninjas have been registered");

        string race = get<string>(idata["Race"]);

        int chance_percent = cfinder(config->race_chance_values, race);
        float chance = config->race_chance_values[chance_percent].value;

        int chance_delay = cfinder(config->race_delay_values, race);
        uint32_t delay =
            (uint32_t)config->race_delay_values[chance_delay].value;

        ninjas.emplace(ram_payer, [&](auto &v)
                       {
          v.asset_id = asset_id;
          v.owner = owner;
          v.race = race;
          v.delay_seconds = delay;
          v.last_search.utc_seconds = 0;
          v.status = "Idle"; });
      }
    }

    else if (asset_itr->schema_name == name("professions"))
    {

      auto profession = professions.find(asset_id);
      check(profession == professions.end(), "Already registered");
      if (player != user.end())
      {

        string type = get<string>(idata["Type"]);
        string name = get<string>(idata["name"]);
        uint64_t uses = 60;

        if (type == "Gatherer")
        {
          uint64_t maxvalue = 0;
          uses = 60;
          for (auto var : player->nft_counts.maxProfessions)
          {
            if (var.key == name)
            {
              maxvalue = var.value;
            }
          }
          check(getnftcount(owner, "profession", name) < maxvalue, "Max" + name + "s have been registered");
        }
        else
        {
          if (name != "Engineer")
            uses = 180;
          else
            uses = 90;
        }
        uint64_t last_used = 0;

        time_point_sec last_time = time_point_sec(last_used);

        auto checkd = mdata.find("Work (uses left)");
        if (checkd != mdata.end())
          uses = get<uint64_t>(mdata["Work (uses left)"]);
        else
        {
          mdata["Work (uses left)"] = uses;
          update_assert_on_atomic(owner, asset_id, mdata);
        }

        professions.emplace(ram_payer, [&](auto &v)
                            {
          v.asset_id = asset_id;
          v.owner = owner;
          v.type = type;
          v.name = name;
          v.last_material_search = last_time;
          v.uses_left = uses;
          v.status = "Idle "; });
      }
    }
    else if (asset_itr->schema_name == name("items"))
    {
      auto item = items.find(asset_id);

      check(item == items.end(), "Already registered");

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
          v.profession=0;
          v.equipped = false;
          v.last_material_search = last_time;
          v.uses_left = uses;
          v.status = "Idle"; });
    }
    else
    {
      check(false, "Invalid asset ID");
    }
  }
}
ACTION waxel::startcraft(name account, uint64_t assetID, string mat, bool refining)
{
  require_auth(account);

  auto player = user.find(account.value);
  check(player != user.end(), "Account not registered!");

  auto profess = professions.find(assetID);
  check(profess != professions.end(), "Invalid asset ID");

  check(profess != professions.end(), "Profession not registered!");

  check(profess->owner == account, "Account does not have this ninja");
  auto config = configs.begin();

  check(profess->last_material_search.utc_seconds == 0, "Cannot start another search yet!");

  if (refining)
  {
    check(profess->name == "Carpenter" || profess->name == "Blacksmith" || profess->name == "Tailor", "This profession cannot be used for refining");
    auto rawtorefined = config->rawmat_refined;
    bool chck = false;
    for (auto refined : rawtorefined)
    {
      if (refined.value == mat)
      {
        chck = true;
      }
    }
    check(chck, "Invalid material entered");
    symbol refined_mat_symbol;
    symbol raw_mat_symbol;
    for (auto refined : rawtorefined)
    {
      if (refined.value == mat)
      {
        raw_mat_symbol = symbol(symbol_code(refined.key), 2);
        refined_mat_symbol = symbol(symbol_code(mat), 2);
      }
    }

    addmtbalance(account, (uint64_t)300, raw_mat_symbol, false);
    addmtbalance(account, (uint64_t)100, refined_mat_symbol, true);
  }
  else
  {
    check(profess->name == "Engineer" || profess->name == "Blacksmith", "This profession cannot be used for crafting");
    uint32_t itemID = (uint32_t)stoull(mat);

    auto itemcombo = config->item_combo;
    uint32_t delay = 0;
    bool found = false;
    for (int i = 0; i < itemcombo.size(); i++)
    {
      if (itemcombo[i].template_id == itemID)
      {
        found = true;
        delay = itemcombo[i].delay;

        for (int j = 0; j < itemcombo[i].ingredients.size(); j++)
        {
          asset mat_tocraft = itemcombo[i].ingredients[j];
          addmtbalance(account, mat_tocraft.amount, mat_tocraft.symbol, false);
        }
      }
    }

    check(found, "Invalid item ID");
    check(delay != 0, "Invalid item ID");
  }

  string status = refining ? "Refining%" + mat : "Crafting%" + mat;

  professions.modify(profess, get_self(), [&](auto &v)
                     { v.last_material_search = current_time_point_sec();
  v.status=status; });
}

ACTION waxel::startsearch(name account, uint64_t assetID)
{
  require_auth(account);

  auto player = user.find(account.value);
  check(player != user.end(), "Account not registered!");

  auto ninja = ninjas.find(assetID);
  auto profess = professions.find(assetID);

  if (ninja != ninjas.end())
  {

    check(ninja != ninjas.end(), "Ninja not registered!");

    check(ninja->owner == account, "Account does not have this ninja");

    check(ninja->last_search.utc_seconds == 0, "Cannot start another search yet!");

    ninjas.modify(ninja, get_self(), [&](auto &v)
                  { v.last_search = current_time_point_sec();
  v.status="Searching"; });
  }
  else if (profess != professions.end())
  {

    check(profess->type == "Gatherer", "Can only use gatherer profession to mine materials!");
    check(profess != professions.end(), "Profession not registered!");

    check(profess->owner == account, "Account does not have this ninja");

    check(profess->last_material_search.utc_seconds == 0, "Cannot start another search yet!");
    professions.modify(profess, get_self(), [&](auto &v)
                       { v.last_material_search = current_time_point_sec();
  v.status="Mining"; });
  }
  else
  {
    check(false, "Asset is not registered or invalid!");
  }
}

ACTION waxel::searchforcz(name account, uint64_t ninjaID)
{

  require_auth(account);
  auto player = user.find(account.value);
  check(player != user.end(), "Account not registered!");

  auto ninja = ninjas.find(ninjaID);
  check(ninja != ninjas.end(), "Ninja not registered!");

  check(ninja->owner == account, "Account does not have this ninja");

  uint32_t delay = current_time_point_sec().utc_seconds - ninja->last_search.utc_seconds;
  check(ninja->last_search.utc_seconds != 0, "Start search before claiming citizen!");

  check(delay >= ninja->delay_seconds, "Cannot claim citizen yet!");
  auto size = transaction_size();

  char buf[size];

  auto read = read_transaction(buf, size);
  check(size == read, "read_transaction() has failed.");

  checksum256 tx_id = eosio::sha256(buf, read);

  uint64_t signing_value;

  memcpy(&signing_value, tx_id.data(), sizeof(signing_value));

  ninjas.modify(ninja, get_self(), [&](auto &v)
                { v.status = "holdup"; });

  action(permission_level{get_self(), name("active")}, name("orng.wax"),
         name("requestrand"),
         std::make_tuple(ninjaID, // used as assoc id
                         signing_value, get_self()))
      .send();
}