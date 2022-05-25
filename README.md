# Waxel World
## Main Screens
![application-flow-diagram](./screenshots/Screenshot_1.png)
![application-flow-diagram](./screenshots/Screenshot_2.png)
![application-flow-diagram](./screenshots/Screenshot_3.png)
![application-flow-diagram](./screenshots/Screenshot_4.png)
## On Process
--meantime
no settlement effect: smartcontract-l
exchange of image
--now
vroomrecu


inventory = warehouse
item = workshop

## waxel-world
Repository for the P2E game Waxel World
unity version: 2020.3.29f1
## important URLs
https://trello.com/b/1zJOkxeM/kanban-template
fliudmyla1@gmail.com
goodnight!2#
https://docs.google.com/spreadsheets/d/1DcxcJhV9RqWeUEW4F2uzOAQblu_NhY_7N4PrerYexO4/edit#gid=1751115604
https://miro.com/app/board/o9J_l8Qhkng=/
https://itch.io/game/edit/1433371
Liudmyla
goodnight!2#
https://waxelworld.itch.io/waxel-world-alpha-testnet
https://play.galaxyminers.io/
https://captxak.itch.io/waxel-ninjas/
PW: waxelninjas2022
https://github.com/Ryxmedia/waxel-world
https://test.neftyblocks.com/c/laxewneftyyy
https://test.neftyblocks.com/creator/laxewneftyyy/mint
https://faucet.waxsweden.org/get_token?<account>
## Unity FreeDownloads
https://unity3d.com/get-unity/download/archive
fliudmyla1@gmail.com
GoodNight!2#
thisiswhatiwant8
## about contract
ninjas, camp(citizens, pofession in guess), professions, warehouse, workshop 

configs:
ninjas: ninjas
items: workshop
professions: professions
settlements: warehouse
user:




## Deploy the Waxel World smart contract new / fresh on the test-net with new credentials
1. create waxel accout at https://waxsweden.org/create-testnet-account/
	-- waxelowner12 as a deployer
	{"msg": "succeeded", "keys": {"active_key": {"public": "EOS54qYPpsgojFHqhYxT6vfdoGWA3bFG1BtPgMHyhfnnqipZYxaQj", "private": "5HxgZQvaJmkpTNN6SZi1waowGagCcvdnUeDvEfJekoUYryhwFKR"}, "owner_key": {"public": "EOS8QBnVgvVvwqg24sTrQAcnkVFgUfdj8S5DKW3Ey3JK9FnWiVgR2", "private": "5KUHu1Cn4tpbT9m3Nb55uM2w7tctH7G4ty1qWQG6nfvPZZ8V7PU"}}, "account": "waxelowner12"}
	-- waxeltest123 as a test
	{"msg": "succeeded", "keys": {"active_key": {"public": "EOS6uBBvLbd6yFeare7qmBD4ecXXXmmh5Qz96ZCTksJ9Xq11Gwb4C", "private": "5JgBPZynEx6rpuCVkcpzZJouGAVGE637JkTXwb8iKqsH3Macm4q"}, "owner_key": {"public": "EOS6pnNwufiPCgnGX2BahBEYLjpLteQdWF3P2aWZwyby5QiGVXAgj", "private": "5JiwFs3chUCRnaHhUpRkbtXJXVQJzWm7eNca5h7FomfDVF63ACf"}}, "account": "waxeltest123"}
	-- waxeltest134 as a test
	{"msg": "succeeded", "keys": {"active_key": {"public": "EOS8dQXZtAYyqYdh2gxYR4jada9ApEfk7xerzWU1Y6kHYhwLxW2yR", "private": "5KFdo6GcpB4m9xvNBPr9Ykkoi6qSsCSPWuacYD4QayQfYC4jKiu"}, "owner_key": {"public": "EOS6gpGBwXoKWnhwUsNSS1NejRMH1cnkWGKWx2yPVfa1vDzbBccCt", "private": "5JnDBux78W9MXMPvfSLqqmSVKwop5u55bKSUudQXnXknQNJBui7"}}, "account": "waxeltest134"}
2. create and getting wallet password
cleos wallet create -n waxelwallet --to-console &&
cleos wallet import -n waxelwallet --private-key 5HxgZQvaJmkpTNN6SZi1waowGagCcvdnUeDvEfJekoUYryhwFKR &&
cleos wallet import -n waxelwallet --private-key 5KUHu1Cn4tpbT9m3Nb55uM2w7tctH7G4ty1qWQG6nfvPZZ8V7PU
3. deploying contract to the deployer account
cd waxel
cleos wallet open -n waxelwallet &&
cleos wallet unlock -n waxelwallet --password PW5K8iKjxvzCByAqDa6pjn6sJSGT5Xg7B7ae75z8vB7nu8fH7aUvk
cleos -u https://testnet.waxsweden.org set contract waxelowner12 $(pwd) waxel.wasm waxel.abi
--testnet: https://testnet.waxsweden.org
--mainnet: https://api.waxsweden.org
4. check out if contracts have been deployed
--testnet: https://wax-test.bloks.io/
cleos -u https://testnet.waxsweden.org system buyram waxelowner12 waxelowner12 "400.00000000 WAX"
cleos -u https://testnet.waxsweden.org set account permission waxelowner12 owner --add-code
cleos -u https://testnet.waxsweden.org set account permission waxelowner12 active --add-code

5. setting configs
---race_chance_values:
[ { "key": "Human", "value": "85.00" }, { "key": "Orc", "value": "87.00" }, { "key": "Undead", "value": "89.00" }, { "key": "Elf", "value": "92.00" } ,{ "key": "Demon", "value": "95.00" }]
---race_delay_values:
[ { "key": "Human", "value": "30.00" }, { "key": "Orc", "value": "25.00" }, { "key": "Undead", "value": "20.00" }, { "key": "Elf", "value": "15.00" } ,{ "key": "Demon", "value": "10.00" }]
---rawmat_chances:
[ { "key": "Common", "value": "80.00" }, { "key": "Uncommon", "value": "17.00" }, { "key": "Rare", "value": "3.00" }]
---rawmat_rarities:
[ { "key": "Common", "values": ["CO","BIRCH","COTTON"] },
 { "key": "Uncommon", "values": ["TO","OAK","FLAX"] }, 
 { "key": "Rare", "values": ["IO","TEAK","SWORMS"]}]
---rawmat_refined:
[ { "key": "CO", "value": "CB" },{ "key": "BIRCH", "value": "BWOOD" },{ "key": "COTTON", "value": "WOOL" },
{ "key": "TO", "value": "TBAR" },{ "key": "OAK", "value": "OWOOD" },{ "key": "FLAX", "value": "LINEN" },
{ "key": "IO", "value": "IBAR" },{ "key": "TEAK", "value": "TWOOD" },{ "key": "SWORMS", "value": "SILK" } ]
---item_combo:
[ { "template_id": "274497","type":"Luck", "ingredients": ["1.00 CB","1.00 BWOOD","1.00 WOOL"] }, 
 { "template_id": "274498", "type":"Luck","ingredients": ["1.00 CB","1.00 BWOOD","1.00 WOOL"] }, 
 { "template_id": "274499", "type":"Luck","ingredients": ["1.00 CB","1.00 BWOOD","1.00 WOOL"] }, 

 { "template_id": "274507", "type":"Double","ingredients": ["2.00 CB","2.00 BWOOD","1.00 WOOL"] }, 
 { "template_id": "274508", "type":"Double","ingredients": ["2.00 CB","2.00 BWOOD","1.00 WOOL"] }, 
 { "template_id": "274525","type":"Double", "ingredients": ["2.00 CB","2.00 BWOOD","1.00 WOOL"] }, 

 { "template_id": "274516", "type":"Extra","ingredients": ["2.00 CB","4.00 BWOOD","2.00 WOOL"] }, 
 { "template_id": "279139","type":"Extra", "ingredients": ["2.00 CB","4.00 BWOOD","2.00 WOOL"] }, 
 { "template_id": "274518", "type":"Extra","ingredients": ["2.00 CB","4.00 BWOOD","2.00 WOOL"] }, 

 { "template_id": "274500", "type":"Luck","ingredients": ["1.00 TBAR","1.00 OWOOD","1.00 LINEN"] },
 { "template_id": "274501", "type":"Luck","ingredients": ["1.00 TBAR","1.00 OWOOD","1.00 LINEN"] }, 
 { "template_id": "274503", "type":"Luck","ingredients": ["1.00 TBAR","1.00 OWOOD","1.00 LINEN"] }, 

 { "template_id": "274510","type":"Double", "ingredients": ["2.00 TBAR","2.00 OWOOD","1.00 LINEN"] },
 { "template_id": "274511","type":"Double", "ingredients": ["2.00 TBAR","2.00 OWOOD","1.00 LINEN"] },
 { "template_id": "274512", "type":"Double","ingredients": ["2.00 TBAR","2.00 OWOOD","1.00 LINEN"] },
 
{ "template_id": "274519","type":"Extra", "ingredients": ["2.00 TBAR","4.00 OWOOD","2.00 LINEN"] },
 { "template_id": "279137","type":"Extra", "ingredients": ["2.00 TBAR","4.00 OWOOD","2.00 LINEN"] },
 { "template_id": "274521", "type":"Extra","ingredients": ["2.00 TBAR","4.00 OWOOD","2.00 LINEN"] },

 { "template_id": "274504","type":"Luck", "ingredients": ["1.00 IBAR","1.00 TWOOD","1.00 SILK"] },
 { "template_id": "274505","type":"Luck", "ingredients": ["1.00 IBAR","1.00 TWOOD","1.00 SILK"] },
 { "template_id": "274506","type":"Luck", "ingredients": ["1.00 IBAR","1.00 TWOOD","1.00 SILK"] },

 { "template_id": "274513","type":"Double", "ingredients": ["2.00 IBAR","2.00 TWOOD","1.00 SILK"] },
 { "template_id": "274514","type":"Double", "ingredients": ["2.00 IBAR","2.00 TWOOD","1.00 SILK"] },
 { "template_id": "274515","type":"Double", "ingredients": ["2.00 IBAR","2.00 TWOOD","1.00 SILK"] },

 { "template_id": "274522", "type":"Extra","ingredients": ["1.00 IBAR","4.00 TWOOD","2.00 SILK"] },
 { "template_id": "279138","type":"Extra", "ingredients": ["1.00 IBAR","4.00 TWOOD","2.00 SILK"] },
 { "template_id": "274524","type":"Extra", "ingredients": ["1.00 IBAR","4.00 TWOOD","2.00 SILK"] }
]
---profession_mats:
[ { "key": "Miner", "values": ["CO","TO","IO"] },
 { "key": "Lumberjack", "values": ["BIRCH","OAK","TEAK"] }, 
 { "key": "Farmer", "values": ["COTTON","FLAX","SWORMS"]}]
---template_ids:
[ { "key": "Citizen10X", "value": "263183" }, { "key": "Miner", "value": "274489" }, { "key": "Lumberjack", "value": "274490" }, 
  { "key": "Farmer", "value": "274491" }, { "key": "Blacksmith", "value": "274492" }, { "key": "Tailor", "value": "274493" }, 
  { "key": "Carpenter", "value": "274494" }, { "key": "Engineer", "value": "274495" }
  , { "key": "CO", "value": "266573" }, { "key": "BIRCH", "value": "266574" }, { "key": "COTTON", "value": "266575" }, { "key": "TO", "value": "266576" }
  , { "key": "OAK", "value": "266577" }, { "key": "FLAX", "value": "266579" }, { "key": "IO", "value": "266580" }, { "key": "TEAK", "value": "266581" }
  , { "key": "SWORMS", "value": "266582" }, { "key": "CB", "value": "266583" }, { "key": "BWOOD", "value": "266584" }, { "key": "WOOL", "value": "266585" }
  , { "key": "TBAR", "value": "266586" }, { "key": "OWOOD", "value": "266587" }, { "key": "LINEN", "value": "266588" }, { "key": "IBAR", "value": "266589" }
  , { "key": "TWOOD", "value": "266590" }, { "key": "SILK", "value": "266592" }
]
---profession_combo:
[ { "type": "Gatherer", "profession": "Miner", "template_id": "274567" }, { "type": "Gatherer", "profession": "Lumberjack", "template_id": "274569" }, { "type": "Gatherer", "profession": "Farmer", "template_id": "274570" }, { "type": "Refiner and crafter", "profession": "Blacksmith", "template_id": "274571" }, { "type": "Refiner and crafter", "profession": "Tailor", "template_id": "274574" }, { "type": "Refiner and crafter", "profession": "Carpenter", "template_id": "274573" }, { "type": "Refiner and crafter", "profession": "Engineer", "template_id": "274575" } ] 



