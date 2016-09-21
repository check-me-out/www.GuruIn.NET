// Tell JSHint to suppress errors relating to objects declared elsewhere.
/* global $, productListApi, productListMvcViewModel */

// Define the `EcommerceModule` module.
var eCommerceModule = angular.module('EcommerceModule', []);

// Define the `productList` component on the `phonecatModule` module.
// The component contains a template & a controller that populates VM for the template.
eCommerceModule.component('productList', {

    // To use cshtml templates, create an Action that provides you the Razor view and use the Action URL here.
    templateUrl: 'Content/Site/Ecommerce/product-list.template.html',

    controller: function ProductListController($http) {
        var self = this;

        self.Products = [];
        self.OrderProperty = 'NewPrice';

        // Retrieve data
        productListApi.client
            .getProducts($http)
            .then(function (response) {
                self.Products = response.data;
            });

        // Filtering
        self.PriceRange = function (item) {
            return (!self.FromPrice || item.NewPrice >= self.FromPrice)
                && (!self.ToPrice || item.NewPrice <= self.ToPrice);
        };
    }
});
