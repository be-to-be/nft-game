
#include <stdint.h>
#include <string>
#include <waxel.hpp>
#include "randomness_provider.cpp"

static inline time_point_sec current_time_point_sec()
{
  return time_point_sec(current_time_point());
};

ACTION waxel::receiverand(uint64_t assoc_id, checksum256 random_value)
{
  require_auth(name("orng.wax"));

  uint64_t max_value = 100;

  RandomnessProvider randomness_provider(random_value);

  auto ninja = ninjas.find(assoc_id); // For search citizen
  if (ninja != ninjas.end())
  {
    uint32_t rand = randomness_provider.get_rand(max_value);

    // the id for roll is selected randomly from the available ones, which
    // points to the assets IDs included with it
    uint64_t selected_roll = (uint64_t)rand;

    action(permission_level{get_self(), name("active")}, get_self(),
           name("logsearch"),
           std::make_tuple(assoc_id, max_value, selected_roll))
        .send();
  }
  else
  {
    auto profess = professions.find(assoc_id);
    if (profess != professions.end())
    {
      if (profess->status != "Burnt")
      {
        if (profess->type == "Gatherer")
        { // For finding material
          uint32_t rand1 = randomness_provider.get_rand(max_value * 100);
          uint32_t rand2 = randomness_provider.get_rand(max_value);
          action(permission_level{get_self(), name("active")}, get_self(),
                 name("logmine"),
                 std::make_tuple(assoc_id, max_value, rand1, rand2))
              .send();
        }
      }
      else
      { // Item burn rng
        uint32_t rand2 = randomness_provider.get_rand(max_value);
        action(permission_level{get_self(), name("active")}, get_self(),
               name("burnrng"), std::make_tuple(assoc_id, max_value, rand2, 0, 0))
            .send();
      }
    }
    else
    {
      auto item = items.find(assoc_id);
      if (item != items.end())
      {
        uint32_t rand5 = randomness_provider.get_rand(max_value);
        uint32_t randb = randomness_provider.get_rand(max_value);
        string memo = item->status;
        size_t start;
        size_t end = 0;
        char delim = '%';
        vector<string> out;
        while ((start = memo.find_first_not_of(delim, end)) !=
               string::npos)
        {
          end = memo.find(delim, start);
          out.push_back(memo.substr(start, end - start));
        }
        string type = out[0];
        if (type == "Burnt")
        {
          uint64_t id = (uint64_t)stoull(out[1]);
          auto config = configs.begin();
          auto itemcombo = config->item_combo;
          uint32_t randa = randomness_provider.get_rand(max_value);
          for (int i = 0; i < itemcombo.size(); i++)
          {
            if (itemcombo[i].template_id == id)
            {
              uint32_t randb =
                  randomness_provider.get_rand(itemcombo[i].ingredients.size());
            }
          }
          action(permission_level{get_self(), name("active")}, get_self(),
                 name("burnrng"),
                 std::make_tuple(assoc_id, max_value, randa, randb))
              .send();
        }
      }
    }
  }
}

ACTION waxel::logmine(uint64_t assoc_id, uint64_t max_value, uint32_t rand1,
                      uint32_t rand2)
{
  require_auth(get_self());
  auto profess = professions.find(assoc_id);

  auto player = user.find(profess->owner.value);

  auto config = configs.begin();

  auto data = profess->items;
  pair_string_float32 luck, doubleluck, more;
  doubleluck.value = 0;
  more.value = 0;

  atomicassets::ATTRIBUTE_MAP mdata = get_mutabledata(profess->owner, assoc_id);

  for (int i = 0; i < data.size(); i++)
  {
    auto item = items.find(data[i]);
    atomicassets::ATTRIBUTE_MAP mdata1 =
        get_mutabledata(profess->owner, data[i]);
    uint64_t uses = item->uses_left;
    uses = uses - 1;

    items.modify(item, get_self(), [&](auto &v)
                 {
      v.last_material_search.utc_seconds = 0;
      v.uses_left -= 1;
      v.status = "Mine success"; });
    mdata1["Durability (uses left)"] = uses;
    update_assert_on_atomic(profess->owner, data[i], mdata1);

    if (item->function.key == "Luck")
      luck = item->function;
    if (item->function.key == "Double")
      doubleluck = item->function;
    if (item->function.key == "Extra")
      more = item->function;
  }

  auto mat_chances = config->rawmat_chances;
  auto mat_rarities = config->rawmat_rarities;
  auto profession_mats = config->profession_mats;

  vector<string> profession_tomat;
  for (auto var : profession_mats)
  {
    if (var.key == profess->name)
    {
      profession_tomat = var.values;
    }
  }
  string mat_rarity = "Common";

  uint32_t counter = rand1;

  uint64_t lamount = luck.value;
  if (lamount!= 0)
  {
    // Modify mat_chances with given %
  }
  for (auto var : mat_chances)
  {
    if (var.value * 100 <= counter)
    {
      mat_rarity = var.key;
      counter -= var.value * 100;
    }
    else
      break;
  }

  vector<string> matarray;

  for (auto var : mat_rarities)
  {
    if (var.key == mat_rarity)
    {
      matarray = var.values;
    }
  }

  string mat = "None";
  for (auto p : profession_tomat)
  {
    for (auto var : matarray)
    {
      if (p == var)
      {
        mat = p;
        break;
      }
    }
  }

  uint64_t amount = 100;
  amount += more.value;

  if (rand2 < doubleluck.value)
  {
    amount += 100;
  }
  symbol mat_symbol = symbol(symbol_code(mat), 2);

  string s_name = profess->name;
  string a_nft = "";
  if (s_name == "Miner")
    a_nft = "Mine";
  if (s_name == "Lumberjack")
    a_nft = "Forest";
  if (s_name == "Farmer")
    a_nft = "Field";
  uint64_t s_count = getnftcount(profess->owner, "settlements", a_nft);
  if (s_count >= 1)
    amount += 5;

  string result = mat + to_string(amount);
  addmtbalance(profess->owner, amount, mat_symbol, true);

  uint64_t uses = profess->uses_left;
  uses = uses - (uint64_t)1;
  professions.modify(profess, get_self(), [&](auto &v)
                     {
    v.last_material_search.utc_seconds = 0;
    v.uses_left = uses;
    v.status = "Mine success- Mined " + result; });
  mdata["Work (uses left)"] = uses;
  update_assert_on_atomic(profess->owner, assoc_id, mdata);
}

ACTION waxel::burnrng(uint64_t assoc_id, uint64_t max_value, uint32_t rand1,
                      uint32_t rand2, uint32_t templateID)
{
  require_auth(get_self());

  auto profess = professions.find(assoc_id);
  if (profess != professions.end())
  {
    auto player = user.find(profess->owner.value);
    if (player != user.end() && profess->status == "Burnt")
    {
      if (rand1 < 10)
      {
        int cz_count = player->citizen_count;
        cz_count += 1;
        user.modify(player, get_self(),
                    [&](auto &v)
                    { v.citizen_count = cz_count; });
      }
    }
    vector<uint64_t> ids;
    ids.push_back(assoc_id);

    action(permission_level{get_self(), name("active")}, get_self(),
           name("unbindnfts"), std::make_tuple(ids, profess->owner))
        .send();
  }
  else
  {
    auto item = items.find(assoc_id);
    if (item != items.end())
    {

      auto player = user.find(item->owner.value);
      if (player != user.end())
      {
        if (rand1 < 10)
        {
          auto config = configs.begin();
          auto itemcombo = config->item_combo;
          for (int i = 0; i < itemcombo.size(); i++)
          {
            if (itemcombo[i].template_id == templateID)
            {
              asset mat_tocraft = itemcombo[i].ingredients[rand2];
              uint64_t amount = 100;
              addmtbalance(item->owner, amount, mat_tocraft.symbol, true);
              break;
            }
          }
        }
      }
      vector<uint64_t> ids;
      ids.push_back(assoc_id);
      action(permission_level{get_self(), name("active")}, get_self(),
             name("unbindnfts"), std::make_tuple(ids, item->owner))
          .send();
    }
  }
}

ACTION waxel::logsearch(uint64_t assoc_id, uint64_t max_value,
                        uint64_t final_value)
{
  require_auth(get_self());

  auto ninja = ninjas.find(assoc_id);

  auto player = user.find(ninja->owner.value);

  auto config = configs.begin();

  int chance_percent = cfinder(config->race_chance_values, ninja->race);

  float chance = config->race_chance_values[chance_percent].value;

  if (final_value <= chance)
  {
    ninjas.modify(ninja, get_self(), [&](auto &v)
                  {
        v.last_search.utc_seconds = 0;
        v.status = "Search successful"; });
    addczbalance(ninja->owner, 1);
  }
  else
  {
    ninjas.modify(ninja, get_self(), [&](auto &v)
                  {
        v.last_search.utc_seconds = 0;
        v.status = "Search failed"; });
  }
}

void waxel::logchange(const name &collection_name, const name &from, name &to,
                      const vector<uint64_t> &asset_ids, const string &memo)
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
      auto profess = professions.find(asset_id);
      if (profess != professions.end())
        profess = professions.erase(profess);
      else
      {
        auto item = items.find(asset_id);
        if (item != items.end())
        {
          if (item->equipped)
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

void waxel::logmintasset(
    uint64_t asset_id, name authorized_minter, name collection_name,
    name schema_name, int32_t template_id, name new_asset_owner,
    ATTRIBUTE_MAP immutable_data, ATTRIBUTE_MAP mutable_data,
    vector<asset> backed_tokens, ATTRIBUTE_MAP immutable_template_data) {}

void waxel::logburnasset(
    name asset_owner, uint64_t asset_id, name collection_name,
    name schema_name, int32_t template_id, vector<asset> backed_tokens,
    ATTRIBUTE_MAP old_immutable_data, ATTRIBUTE_MAP old_mutable_data,
    name asset_ram_payer)
{

  vector<uint64_t> ids;
  ids.push_back(asset_id);

  auto config = configs.begin();
  auto size = transaction_size();

  char buf[size];

  auto read = read_transaction(buf, size);
  check(size == read, "read_transaction() has failed.");

  checksum256 tx_id = eosio::sha256(buf, read);

  uint64_t signing_value;

  memcpy(&signing_value, tx_id.data(), sizeof(signing_value));

  auto ninja = ninjas.find(asset_id);
  if (ninja != ninjas.end())
  {
    ninja = ninjas.erase(ninja);
  }
  if (schema_name == ITEM_SCHEMA)
  {

    auto item = items.find(asset_id);
    if (item != items.end())
    {
      items.modify(item, get_self(), [&](auto &v)
                   { v.status = "Burnt%" + to_string(template_id); });

      action(permission_level{get_self(), name("active")}, name("orng.wax"),
             name("requestrand"),
             std::make_tuple(asset_id, // used as assoc id
                             signing_value, get_self()))
          .send();
    }
  }
  if (schema_name == PROFESSION_SCHEMA) //Burn RNG
  {

    auto profession = professions.find(asset_id);
    if (profession != professions.end())
    {
      professions.modify(profession, get_self(),
                         [&](auto &v)
                         { v.status = "Burnt"; });
      action(permission_level{get_self(), name("active")}, name("orng.wax"),
             name("requestrand"),
             std::make_tuple(asset_id, // used as assoc id
                             signing_value, get_self()))
          .send();
    }
  }

  if (schema_name == CITIZEN_SCHEMA && template_id ==CZ_PACK_TEMPLATE ) // Temporary template added
  {
    addczbalance(asset_owner, (int)10);
  }

  if (schema_name ==MPACK_SCHEMA)
  {
    auto config = configs.begin();
    auto t_ids = config->template_ids;
    string key = "CO";
    for (auto id : t_ids)
    {
      if ((uint64_t)template_id == id.value)
      {
        key = id.key;
      }
    }
    symbol mat = symbol(symbol_code(key), 2);
    addmtbalance(asset_owner, (uint64_t)1000, mat, true);
  }
  /**/
}