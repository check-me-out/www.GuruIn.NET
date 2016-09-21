// Tell JSHint to suppress errors relating to objects declared elsewhere.
/* global $, productListApi, productListMvcViewModel */

var productListApi = productListApi || {};
var productListMvcViewModel = productListMvcViewModel || null;

productListApi.client = (function (productListApiEndpoint) {

    var getProducts = function getAllProducts($http) {
        var endpoint = productListApiEndpoint + '/GetAllProducts';
        return $http.get(endpoint);
    };

    return {
        getProducts: getProducts
    };

}(productListMvcViewModel.ProductListApiEndpoint));
