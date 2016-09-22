// Tell JSHint to suppress errors relating to objects declared elsewhere.
/* global $http, EcommerceService, EcommerceMvcViewModel */

var EcommerceMvcViewModel = EcommerceMvcViewModel || null;

{
    var EcommerceService = EcommerceService || {};
    EcommerceService.api = function (restful, eCommerceApiEndpoint) {

        var getAllProducts = function getAllProducts() {
            var endpoint = eCommerceApiEndpoint + '/GetAllProducts';
            return restful.get(endpoint);
        };

        var getProduct = function getAllProducts(productId) {
            var endpoint = eCommerceApiEndpoint + '/GetProduct/' + productId;
            return restful.get(endpoint);
        };

        return {
            getAllProducts: getAllProducts,
            getProduct: getProduct
        };

    };

    angular.module('ecommService', ['ngRoute'])
           .config(['$provide',
               function ($provide) {
                   $provide.factory('EcommerceService', ['$http',
                       function ($http) {
                           return EcommerceService.api($http, EcommerceMvcViewModel.EcommerceApiEndpoint);
                       }
                   ])
               }
           ]);
}
