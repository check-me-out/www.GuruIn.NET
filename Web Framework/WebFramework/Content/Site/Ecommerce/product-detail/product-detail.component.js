// Tell JSHint to suppress errors relating to objects declared elsewhere.
/* global $routeParams, ProductDetailModule */

// Get `productDetail` module.
var ProductDetailModule = ProductDetailModule || angular.module('productDetail', ['ngRoute']);

// Define the `productDetail` component on the `productDetail` module.
// The component contains a template & a controller that populates VM for the template.
ProductDetailModule.component('productDetail', {

      // To use cshtml templates, create an Action that provides you the Razor view and use the Action URL here.
      templateUrl: 'Content/Site/Ecommerce/product-detail/product-detail.template.html',

      controller: ['$routeParams', 'EcommerceService',
          function ProductListController($routeParams, EcommerceService) {
              var self = this;

              self.Product = {};
              self.OrderProperty = 'NewPrice';
              self.SelectedImageUrl = '';

              // Retrieve data
              EcommerceService.getProduct($routeParams.productId)
                              .then(function (response) {
                                  self.Product = response.data;
                                  if (self.Product) {
                                      self.SelectedImageUrl = self.Product.ImageUrl;
                                  }
                              });

              self.ChangeImageSelection = function (newImageUrl) {
                  // Temp solution - not correct to access DOM (presentation layer) as this func is now un-testable.
                  $(".selected-image img").fadeOut(750, function () {
                      $(".selected-image img").fadeIn(50, function () {
                          self.SelectedImageUrl = newImageUrl;
                      });
                  });
              };
          }
      ]
  });
