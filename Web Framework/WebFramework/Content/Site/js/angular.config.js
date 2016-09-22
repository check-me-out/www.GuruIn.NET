'use strict';

// Define the `EcommerceModule` module
angular.module('EcommerceModule', [
                'ngRoute',
                'siteSpecific',
                'productDetail',
                'productList',
            ])
        .config(['$locationProvider', '$routeProvider',
            function config($locationProvider, $routeProvider) {
                $locationProvider.hashPrefix('!');

                $routeProvider.
                  when('/products', {
                      template: '<product-list></product-list>'
                  }).
                  when('/products/:productId', {
                      template: '<product-detail></product-detail>'
                  }).
                  otherwise('/products');
            }
        ]);

angular.module('siteSpecific', ['ecommService']);
