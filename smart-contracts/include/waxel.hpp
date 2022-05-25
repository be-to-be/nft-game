#include <atomicassets.hpp>
#include <atomicdata.hpp>
#include <cstdint>
#include <eosio/asset.hpp>
#include <eosio/crypto.hpp>
#include <eosio/eosio.hpp>
#include <eosio/transaction.hpp>
#include <stdint.h>
#include <string>


using namespace eosio;
using namespace std;

#define ATOMIC name("atomicassets")
#define EOSIO name("eosio")

CONTRACT waxel : public contract {
public:
  using contract::contract;

  const name COLLECTION_NAME = name("laxewneftyyy");
  const int RELEASE_DELAY = 20;
  const name NINJA_SCHEMA = name("laxewnefty");
  const name PROFESSION_SCHEMA = name("professions");
  const name CITIZEN_SCHEMA = name("citizens");
  const name ITEM_SCHEMA = name("items");
  const name BOOKS_SCHEMA = name("books");
  const name UPGRADE_SCHEMA = name("upgrades");
  const name MPACK_SCHEMA = name("upgrades");
  const int CRAFTING_DELAY = 30;
  const int FINDING_DELAY = 20;
  const int REFINING_DELAY = 30;
const uint64_t CZ_PACK_TEMPLATE=263183;
  struct pair_string_float32 {
    string key;
    float value;
  };

  struct pair_string_uint64 {
    string key;
    uint64_t value;
  };

  struct pair_int {
    vector<pair_string_uint64> maxProfessions;
    int maxNinja;
  };

  struct pair_string_string {
    string key;
    string value;
  };

  struct pair_string_vecstring {
    string key;
    vector<string> values;
  };

  struct citizen_s {
    vector<uint64_t> asset_ids;
  };

  struct profession_blend {
    string type;
    string profession;
    uint64_t template_id;
  };

  struct inventory {
    uint64_t profession;
    vector<uint64_t> items;
  };

  struct mat_bag {
    string type;
    vector<pair_string_uint64> items;
  };

  struct pair_int_vecasset
  {
    uint64_t template_id;
    string type;
    uint32_t delay;
    vector<asset> ingredients;
  };


  // log search finding citizen
  ACTION logsearch(uint64_t assoc_id, uint64_t max_value, uint64_t final_value);

  // Receive callback from orng.wax after mining, finding citizen or burn RNG
  ACTION receiverand(uint64_t assoc_id, checksum256 random_value);

  //Start craft for professions
  ACTION startcraft(name account, uint64_t assetID,string mat,bool refining);
  ACTION equipitems(uint64_t profession_id, vector<uint64_t> item_ids);


  //Admin actions
  ACTION setconfig(vector<pair_string_float32> race_chance_values,
                   vector<pair_string_float32> race_delay_values,
                   vector<pair_string_float32> rawmat_chances,
                   vector<pair_string_vecstring> rawmat_rarities,
                   vector<pair_string_string> rawmat_refined,
                   vector<pair_int_vecasset> item_combo,
                   vector<pair_string_vecstring> profession_mats,
                   vector<pair_string_uint64> template_ids,
                   vector<profession_blend> profession_combo);

  ACTION removeall(string table);

  //Testing action
  ACTION fillmat(name owner);

  // mint citizens in user inventory to nft template
  ACTION mintcitizens(name account,int amount);

  //Log mine result and process calculations
  ACTION logmine(uint64_t assoc_id, uint64_t max_value, uint32_t rand1,
                 uint32_t rand2);


  //User actions

  // Find materials by gatherer professions
  ACTION findmat(name account, uint64_t assetID);

  // ninja finding citizen
  ACTION searchforcz(name account, uint64_t assetID);

  // register ninja,profession, item
  ACTION registernfts(vector<uint64_t> asset_ids, name owner);

  // Craft materials to item by refiner and crafter profession
  ACTION craftitem(name account, uint64_t professID);

  //Refine material by refiner profession
  ACTION refinemat(name account, uint64_t professID);

  //Unequip items from profession
  ACTION unequipitems( vector<uint64_t> item_ids);

  //Start search for finding mat, refining or crafting in professions
  ACTION startsearch(name account, uint64_t assetID);

  //Withdraw settlement NFTs
  ACTION withdrawnfts(name account, vector<uint64_t> asset_ids);

  //Unregister ninja, professiona or item NFTs
  ACTION unbindnfts(vector<uint64_t> asset_ids, name owner);

  //Mint material pack for materials with amount
  ACTION mintmats(name account,string mat,int amount);

  //Reset data for user (temporary)
  ACTION resetuser(name account);

  //Log action for burn rng after receiving random value
  ACTION burnrng(uint64_t assoc_id, uint64_t max_value, uint32_t rand1, uint32_t rand2, uint32_t templateID);


  //Atomic assets notify actions <Triggered on transfer, burn or mint of a new asset>
  [[eosio::on_notify("atomicassets::logtransfer")]] void logchange(
      const name &collection_name, const name &from, name &to,
      const vector<uint64_t> &asset_ids, const string &memo);

  [[eosio::on_notify("atomicassets::transfer")]] void receive_asset_transfer(
      name from, name to, vector<uint64_t> asset_ids, string memo);

  //
  [[eosio::on_notify("atomicassets::logburnasset")]] void logburnasset(
      name asset_owner, uint64_t asset_id, name collection_name,
      name schema_name, int32_t template_id, vector<asset> backed_tokens,
      atomicassets::ATTRIBUTE_MAP old_immutable_data,
      atomicassets::ATTRIBUTE_MAP old_mutable_data, name asset_ram_payer);

  [[eosio::on_notify("atomicassets::logmint")]] void logmintasset(
      uint64_t asset_id, name authorized_minter, name collection_name,
      name schema_name, int32_t template_id, name new_asset_owner,
      ATTRIBUTE_MAP immutable_data, ATTRIBUTE_MAP mutable_data,
      vector<asset> backed_tokens, ATTRIBUTE_MAP immutable_template_data);

private:

  //Used to store all configuration related details;
  TABLE config_t {
    uint64_t id;
    vector<pair_string_float32> race_chance_values;
    vector<pair_string_float32> race_delay_values;
    vector<pair_string_float32> rawmat_chances;
    vector<pair_string_vecstring> rawmat_rarities;
    vector<pair_string_string> rawmat_refined;
    vector<pair_string_vecstring> profession_mats;
    vector<pair_int_vecasset> item_combo;
    vector<pair_string_uint64> template_ids;
    vector<profession_blend> profession_combo;

    auto primary_key() const { return id; };
  };

  //Store user data
  TABLE user_t {
    name account;
    int citizen_count;
    pair_int nft_counts;
    vector<asset> mat_inventory;
    auto primary_key() const { return account.value; };
  };

  //Store ninja nfts data
  TABLE ninjas_t {
    uint64_t asset_id;
    name owner;
    time_point_sec last_search;
    string race;
    uint32_t delay_seconds;
    string status;
    auto primary_key() const { return asset_id; };
    uint64_t by_account() const { return owner.value; }
  };

//Store profession nfts data
  TABLE professions_t {
    uint64_t asset_id;
    name owner;
    string type;
    string name;
    time_point_sec last_material_search;
    uint64_t uses_left;
    vector<uint64_t> items;
    string status;
    auto primary_key() const { return asset_id; };
    uint64_t by_account() const { return owner.value; }
  };

//Store item nfts data
  TABLE items_t {
    uint64_t asset_id;
    name owner;
    string name;
    uint64_t profession;
    pair_string_float32 function;
    bool equipped;
    time_point_sec last_material_search;
    uint64_t uses_left;
    string status;
    auto primary_key() const { return asset_id; };
    uint64_t by_account() const { return owner.value; }
  };

//Store settlements data
  TABLE settlements_t {
    uint64_t asset_id;
    name owner;
    string type;
    auto primary_key() const { return asset_id; };
    uint64_t by_account() const { return owner.value; }
  };


  typedef eosio::multi_index<name("configs"), config_t> config_s;
  typedef eosio::multi_index<name("user"), user_t> user_s;
  typedef eosio::multi_index<
      name("ninjas"), ninjas_t,
      indexed_by<name("account"),
                 const_mem_fun<ninjas_t, uint64_t, &ninjas_t::by_account>>>
      ninjas_s;

  typedef eosio::multi_index<
      name("professions"), professions_t,
      indexed_by<name("account"), const_mem_fun<professions_t, uint64_t,
                                                &professions_t::by_account>>>
      professions_s;

  typedef eosio::multi_index<
      name("items"), items_t,
      indexed_by<name("account"),
                 const_mem_fun<items_t, uint64_t, &items_t::by_account>>>
      items_s;

  typedef eosio::multi_index<
      name("settlements"), settlements_t,
      indexed_by<name("account"),
                 const_mem_fun<settlements_t, uint64_t, &settlements_t::by_account>>>
      settlements_s;

  ninjas_s ninjas = ninjas_s(get_self(), get_self().value);
  user_s user = user_s(get_self(), get_self().value);
  config_s configs = config_s(get_self(), get_self().value);
  professions_s professions = professions_s(get_self(), get_self().value);
  items_s items = items_s(get_self(), get_self().value);
  settlements_s settlements = settlements_s(get_self(), get_self().value);

  int cfinder(vector<pair_string_float32> data, string key);
  int finder(vector<pair_string_uint64> data, string key);
  string professionfinder(vector<profession_blend> data, uint64_t template_id);
  waxel::profession_blend filterprofession(uint32_t template_id);

  void mintproffess(name account, uint32_t template_id,
                    atomicassets::ATTRIBUTE_MAP idata,
                    atomicassets::ATTRIBUTE_MAP mdata);

void burnassets(vector<uint64_t> asset_ids);
int getnftcount(name account, string type,string variant);
void addczbalance(name account, int count);
void update_assert_on_atomic(name asset_owner, uint64_t asset_id, ATTRIBUTE_MAP mdata) ;
ATTRIBUTE_MAP get_immutabledata(name asset_owner, uint64_t asset_id) ;
ATTRIBUTE_MAP get_mutabledata(name asset_owner, uint64_t asset_id) ;
void addmtbalance(name player, uint64_t amount,symbol mat, bool add);
void blendprofess(name from, name to, vector<uint64_t> asset_ids);
void regupgrade(name from, name to, vector<uint64_t> asset_ids);

void regnewplayer(name owner,name ram_payer);

};