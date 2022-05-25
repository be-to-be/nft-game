cd build
cmake ..
make
cd waxel
cleos wallet open -n waxelwallet &&
cleos wallet unlock -n waxelwallet --password PW5K8iKjxvzCByAqDa6pjn6sJSGT5Xg7B7ae75z8vB7nu8fH7aUvk
cleos -u https://testnet.waxsweden.org set contract waxelowner12 $(pwd) waxel waxel.wasm waxel.abi
