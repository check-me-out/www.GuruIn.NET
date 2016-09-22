// Tell JSHint to suppress errors relating to objects declared elsewhere.
/* global $http, ProductListModule, EcommerceService */

// Get `productList` module.
var ProductListModule = ProductListModule || angular.module('productList', ['ngRoute']);

// Define the `productList` component on the `productList` module.
// The component contains a template & a controller that populates VM for the template.
ProductListModule.component('productList', {

    // To use cshtml templates, create an Action that provides you the Razor view and use the Action URL here.
    templateUrl: 'Content/Site/Ecommerce/product-list/product-list.template.html',

    controller: ['EcommerceService', function ProductListController(EcommerceService) {
        var self = this;

        self.Products = [];
        self.OrderProperty = 'NewPrice';

        // Retrieve data
        EcommerceService.getAllProducts()
                        .then(function (response) {
                            self.Products = response.data;
                        });

        // Filtering
        self.PriceRange = function (item) {
            return (!self.FromPrice || item.NewPrice >= self.FromPrice)
                && (!self.ToPrice || item.NewPrice <= self.ToPrice);
        };
    }]
});
