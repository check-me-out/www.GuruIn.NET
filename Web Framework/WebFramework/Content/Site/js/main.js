requirejs.config({

    baseUrl: 'Content/Site/js',

    paths: {
        'jquery': [
                        'https://code.jquery.com/jquery-2.2.1.min',
                        '../../jQuery/jquery-2.2.1.min' // fallback
                  ],
        'bootstrap': [
                        'https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min',
                        '../../Bootstrap/js/bootstrap.min' // fallback
                     ],
    },

    shim: {
        'bootstrap': {
            deps: ['jquery']
        },
        'error-handler': {
            deps: ['jquery']
        },
        'utils': {
            deps: ['jquery']
        },
        'common': {
            deps: ['utils']
        },
        'centered-layout': {
            deps: ['utils']
        }
    },

    bundles: {
        '~/bundles/centered-layout-js': [ 'error-handler', 'utils', 'common', 'centered-layout' ],
        '~/bundles/fiddle-scripts': ['fiddle-scripts']
        //'bundles/form-data-validation': ['fiddle-scripts']
    }

});
