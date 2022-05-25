mergeInto(LibraryManager.library, {
  autologin: function(){
    autoLogin();
  },
  login: function(type) {
    selectWallet(UTF8ToString(type));
  },
  logout: function() {
    doLogoutAction();
  },
  getAssetData: function() {
    getAssetD();
  },
  getNinjaData: function() {
    getNinjaD();
  },
  searchcz: function(int,type,asset_type) {
     var asset_id  = UTF8ToString(int);
     var type_id =  UTF8ToString(type);
     search_citizen(asset_id,type_id,UTF8ToString(asset_type));                                        
  },
  unregisternft: function(id_u,race_u){
    var u_id  = UTF8ToString(id_u);
    var u_race =  UTF8ToString(race_u);
    unregisterAsset(u_id,u_race);
  },
  registernft: function(int,race_r){
    var r_id  = UTF8ToString(int);
    var r_race = UTF8ToString(race_r); 
    registernft(r_id,r_race);
  },
  mintcitizens: function(){
    mintcitizens();
  },
  burncitizennft: function(){
    burncitizennft();
  },
  asset_transfer: function(t_id,t_memo,t_type){
    var id_t = UTF8ToString(t_id);
    var memo_t = UTF8ToString(t_memo);
    transfer(id_t,memo_t,UTF8ToString(t_type));
  },
  asset_withdraw: function(id_w,type_w){
    var w_id = UTF8ToString(id_w);
    withdraw_asset(w_id,UTF8ToString(type_w));
  },
  equip_items: function(eq_p_id,eq_i_id){
    var id_p = UTF8ToString(eq_p_id);
    var id_i = UTF8ToString(eq_i_id);
    equipItems(id_p,id_i);
  },
  unequip_items: function(un_ids,un_mat_name,un_p_id){
    unequipItems(UTF8ToString(un_ids),UTF8ToString(un_mat_name),UTF8ToString(un_p_id));
  },
  findmat: function(mat_asset_id){
    var asset_id_mat = UTF8ToString(mat_asset_id);
    find_mat(asset_id_mat);
  },
  refine_mat: function(ref_p_id,ref_mat_name,refine_profession_name){
     mat_refine(UTF8ToString(ref_p_id),UTF8ToString(ref_mat_name),UTF8ToString(refine_profession_name));
  },
  refine_comp: function(ref_c_pid){
    mat_refine_comp(UTF8ToString(ref_c_pid));
  },
  craft_mat: function(craft_p_id,craft_mat_id,craft_profession_name){
     mat_craft(UTF8ToString(craft_p_id),UTF8ToString(craft_mat_id),UTF8ToString(craft_profession_name));
  },
  craft_comp: function(craft_c_pid){
    mat_craft_comp(UTF8ToString(craft_c_pid));
  },
  mint_mat: function(mint_matName){
    mintmat(UTF8ToString(mint_matName));
  },
  burn_mat: function(burn_matName){
    burnmat(UTF8ToString(burn_matName));
  },
  burn_items: function(burn_itemName,burn_itemId){
    burn_itemsnft(UTF8ToString(burn_itemName),UTF8ToString(burn_itemId));
  },
  burn_profession: function(burn_professName,burn_professId){
    burn_profession_nft(UTF8ToString(burn_professName),UTF8ToString(burn_professId));
  }
});
