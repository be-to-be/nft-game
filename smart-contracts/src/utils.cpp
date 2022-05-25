#include "logactions.cpp"
#include <stdint.h>

void waxel::mintproffess(name account, uint32_t template_id,
                         atomicassets::ATTRIBUTE_MAP idata,
                         atomicassets::ATTRIBUTE_MAP mdata)
{
  action(permission_level{get_self(), name("active")}, name("atomicassets"),
         name("mintasset"),
         make_tuple(get_self(), COLLECTION_NAME, PROFESSION_SCHEMA, template_id,
                    account, idata, mdata, (vector<asset>){}))
      .send();
}

void waxel::burnassets(vector<uint64_t> asset_ids)
{

  for (int i = 0; i < asset_ids.size(); i++)

    action(permission_level{get_self(), name("active")}, name("atomicassets"),
           name("burnasset"), make_tuple(get_self(), asset_ids[i]))
        .send();
}

void waxel::addczbalance(name account, int count)
{
  auto player = user.find(account.value);
  if (player == user.end() && count > 0)
  {
    pair_int nftdata;
    vector<pair_string_uint64> maxProfessions;
    pair_string_uint64 temp, temp1, temp2;
    temp.key = "Miner";
    temp.value = 5;
    temp1.key = "Lumberjack";
    temp1.value = 5;
    temp2.key = "Farmer";
    temp2.value = 5;
    nftdata.maxNinja = 10;
    maxProfessions.push_back(temp);
    maxProfessions.push_back(temp1);
    maxProfessions.push_back(temp2);
    nftdata.maxProfessions = maxProfessions;
    user.emplace(get_self(), [&](auto &v)
                 {
      v.account = account;
      v.citizen_count = count;
      v.nft_counts=nftdata; });
  }
  else
  {
    check(player != user.end(), "No player found");
    check(player->citizen_count + count > 0, "Exceeded citizen balance");
    user.modify(player, get_self(),
                [&](auto &v)
                { v.citizen_count += count; });
  }
}

void waxel::update_assert_on_atomic(name asset_owner, uint64_t asset_id, ATTRIBUTE_MAP mdata)
{
  action(permission_level{get_self(), name("active")}, name("atomicassets"),
         name("setassetdata"),
         make_tuple(get_self(), asset_owner, asset_id, mdata))
      .send();
}

ACTION waxel::setconfig(vector<pair_string_float32> race_chance_values,
                        vector<pair_string_float32> race_delay_values,
                        vector<pair_string_float32> rawmat_chances,
                        vector<pair_string_vecstring> rawmat_rarities,
                        vector<pair_string_string> rawmat_refined,
                        vector<pair_int_vecasset> item_combo,
                        vector<pair_string_vecstring> profession_mats,
                        vector<pair_string_uint64> template_ids,
                        vector<profession_blend> profession_combo)
{
  require_auth(get_self());

  auto config = configs.begin();
  if (config == configs.end())
  {
    configs.emplace(get_self(), [&](auto &v)
                    {
      v.race_chance_values = race_chance_values;
      v.race_delay_values = race_delay_values;
      v.rawmat_chances = rawmat_chances;
      v.rawmat_rarities = rawmat_rarities;
      v.rawmat_refined = rawmat_refined;
      v.item_combo = item_combo;
      v.profession_mats = profession_mats;
      v.template_ids = template_ids;
      v.profession_combo = profession_combo; });
  }
  else
  {
  }
}

ATTRIBUTE_MAP waxel::get_mutabledata(name asset_owner, uint64_t asset_id)
{
  ATTRIBUTE_MAP mdata = {};
  atomicassets::assets_t owners_assets = atomicassets::get_assets(asset_owner);
  auto idxCard = owners_assets.find(asset_id);
  check(idxCard != owners_assets.end(),
        "This asset is not in the owners assets");
  check(idxCard->collection_name == COLLECTION_NAME,
        "This asset have unsupporteed collection name");

  atomicassets::schemas_t collection_schemas =
      atomicassets::get_schemas(idxCard->collection_name);
  auto idxSchema = collection_schemas.find(idxCard->schema_name.value);

  mdata = atomicdata::deserialize(idxCard->mutable_serialized_data,
                                  idxSchema->format);
  return mdata;
}

ATTRIBUTE_MAP waxel::get_immutabledata(name asset_owner, uint64_t asset_id)
{
  ATTRIBUTE_MAP mdata = {};
  atomicassets::assets_t owners_assets = atomicassets::get_assets(asset_owner);
  auto idxCard = owners_assets.find(asset_id);
  check(idxCard != owners_assets.end(),
        "The asset is not in the owners assets");
  check(idxCard->collection_name == COLLECTION_NAME,
        "This asset have unsupporteed collection name");

  atomicassets::schemas_t collection_schemas =
      atomicassets::get_schemas(idxCard->collection_name);
  auto idxSchema = collection_schemas.find(idxCard->schema_name.value);

  atomicassets::templates_t collection_templates =
      atomicassets::get_templates(idxCard->collection_name);
  auto template_itr = collection_templates.require_find(idxCard->template_id, "Not found");

  vector<uint8_t> immutable_serialized_data =
      template_itr->immutable_serialized_data;
  mdata = atomicdata::deserialize(immutable_serialized_data,
                                  idxSchema->format);
  return mdata;
}

void waxel::regnewplayer(name owner, name ram_payer)
{
  auto player = user.find(owner.value);
  check(player == user.end(), "Already registered!");
  pair_int nftdata;
  vector<pair_string_uint64> maxProfessions;
  pair_string_uint64 temp, temp1, temp2;
  temp.key = "Miner";
  temp.value = 10;
  temp1.key = "Lumberjack";
  temp1.value = 10;
  temp2.key = "Farmer";
  temp2.value = 10;
  nftdata.maxNinja = 10;
  maxProfessions.push_back(temp);
  maxProfessions.push_back(temp1);
  maxProfessions.push_back(temp2);
  nftdata.maxProfessions = maxProfessions;
  user.emplace(ram_payer, [&](auto &v)
               {
      v.account = owner;
      v.citizen_count = 0;
      v.nft_counts=nftdata; });
}

int waxel::getnftcount(name account, string type, string variant)
{

  auto player = user.find(account.value);
  int count = 0;
  if (type == "ninja")
  {

    auto idx = ninjas.get_index<"account"_n>();

    auto itr = idx.lower_bound(account.value);

    while (itr != idx.end())
    {
      if (itr->owner == account)
      {
        count++;
      }
      else
        break;
      itr++;
    }
  }
  else if (type == "profession")
  {
    auto idx = professions.get_index<"account"_n>();

    auto itr = idx.lower_bound(account.value);

    while (itr != idx.end())
    {
      if (itr->owner == account && itr->name == variant)
      {
        count++;
      }
      else if (itr->owner != account)
        break;
      itr++;
    }
  }
  else if (type == "settlements")
  {
    auto idx = settlements.get_index<"account"_n>();

    auto itr = idx.lower_bound(account.value);

    while (itr != idx.end())
    {
      if (itr->owner == account && itr->type == variant)
      {
        count++;
      }
      else if (itr->owner != account)
        break;
      itr++;
    }
  }
  return count;
}

string waxel::professionfinder(vector<profession_blend> data,
                               uint64_t template_id)
{
  for (int i = 0; i < data.size(); i++)
  {
    if (data[i].template_id == template_id)
      return data[i].profession;
  }
  return "";
}

int waxel::cfinder(vector<waxel::pair_string_float32> data, string key)
{
  for (int i = 0; i < data.size(); i++)
  {
    if (key == data[i].key)
      return i;
  }
  return -1;
}

int waxel::finder(vector<waxel::pair_string_uint64> data, string key)
{
  for (int i = 0; i < data.size(); i++)
  {
    if (key == data[i].key)
      return i;
  }
  return -1;
}

waxel::profession_blend
waxel::filterprofession(uint32_t template_id)
{
  auto config = configs.begin();
  auto data = config->profession_combo;

  profession_blend filtered_data;
  for (int i = 0; i < data.size(); i++)
  {
    if (data[i].template_id == template_id)
    {
      filtered_data = data[i];
      break;
    }
  }
  return filtered_data;
}

void waxel::addmtbalance(name player, uint64_t amount, symbol mat, bool add)
{
  auto playerz = user.find(player.value);
  check(playerz != user.end(), "No user found!");
  auto player_mats = playerz->mat_inventory;

  bool found = false;
  symbol mat_symbol = mat;
  for (int i = 0; i < player_mats.size(); i++)
  {
    if (player_mats[i].symbol == mat_symbol)
    {
      if (add)
        player_mats[i].amount += amount;
      else
      {
        check(player_mats[i].amount >= amount, "Exceeded material balance");
        player_mats[i].amount -= amount;
      }
      found = true;
    }
  }
  if (!found)
  {
    if (add)
    {
      asset temp;
      temp.amount = amount;
      temp.symbol = mat;
      player_mats.push_back(temp);
    }
    else
    {
      check(false, "No material found in inventory");
    }
  }

  user.modify(playerz, get_self(),
              [&](auto &v)
              { v.mat_inventory = player_mats; });
}

ACTION waxel::fillmat(name owner)
{

  auto player = user.find(owner.value);

  auto mat_inventory = player->mat_inventory;

  auto config = configs.begin();

  auto rarities = config->rawmat_rarities;
  auto refined = config->rawmat_refined;

  symbol refinesymbol;

  for (auto refine : refined)
  {
    refinesymbol = symbol(symbol_code(refine.value), 2);
    bool refinefound = false;

    for (int i = 0; i < mat_inventory.size(); i++)
    {
      if (mat_inventory[i].symbol == refinesymbol)
      {
        refinefound = true;
        mat_inventory[i].amount += 1000;
      }
    }
    addmtbalance(owner, (uint64_t)100, refinesymbol, true);
    if (!refinefound)
    {
      asset temp;
      temp.symbol = refinesymbol;
      temp.amount = 1000;
      mat_inventory.push_back(temp);
    }
  }

  symbol raw_mat_symbol;
  for (int i = 0; i < rarities.size(); i++)
  {
    for (auto mat : rarities[i].values)
    {
      raw_mat_symbol = symbol(symbol_code(mat), 2);
      bool rawfound = false;

      for (int i = 0; i < mat_inventory.size(); i++)
      {
        if (mat_inventory[i].symbol == raw_mat_symbol)
        {
          rawfound = true;
          mat_inventory[i].amount += 1000;
        }
      }
      addmtbalance(owner, (uint64_t)100, raw_mat_symbol, true);

      if (!rawfound)
      {
        asset temp;
        temp.symbol = raw_mat_symbol;
        temp.amount = 1000;
        mat_inventory.push_back(temp);
      }
    }
  }
  user.modify(player, owner, [&](auto &v)
              {
      v.citizen_count=100;
     v.mat_inventory = mat_inventory; });
}

ACTION waxel::removeall(string table)
{

  require_auth(get_self());

  if (table == "ninjas")
  {
    auto it = ninjas.begin();
    while (it != ninjas.end())
    {
      it = ninjas.erase(it);
    }
  }
  else if (table == "user")
  {
    auto it = user.begin();
    while (it != user.end())
    {
      it = user.erase(it);
    }
  }
  else if (table == "configs")
  {
    auto it = configs.begin();
    while (it != configs.end())
    {
      it = configs.erase(it);
    }
  }
  else if (table == "professions")
  {
    auto it = professions.begin();
    while (it != professions.end())
    {
      it = professions.erase(it);
    }
  }
  else if (table == "items")
  {
    auto it = items.begin();
    while (it != items.end())
    {
      it = items.erase(it);
    }
  }
  else if (table == "settlements")
  {
    auto it = settlements.begin();
    while (it != settlements.end())
    {
      it = settlements.erase(it);
    }
  }
}