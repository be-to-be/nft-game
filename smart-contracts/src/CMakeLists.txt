project(waxel)

set(EOSIO_WASM_OLD_BEHAVIOR "Off")
find_package(eosio.cdt)

add_contract( waxel waxel waxel.cpp )
target_include_directories( waxel PUBLIC ${CMAKE_SOURCE_DIR}/../include )
target_ricardian_directory( waxel ${CMAKE_SOURCE_DIR}/../ricardian )