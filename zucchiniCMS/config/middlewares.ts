import type { Core } from '@strapi/strapi';

const config: Core.Config.Middlewares = [
    'strapi::logger',
    'strapi::errors',

    {
        name: 'strapi::security',
        config: {
            contentSecurityPolicy: {
                useDefaults: true,
                directives: {
                    'connect-src': ["'self'", 'https:'],

                    'img-src': [
                        "'self'",
                        'data:',
                        'blob:',
                        'https://market-assets.strapi.io',
                        'https://zucchinibag.blob.core.windows.net',
                    ],

                    'media-src': [
                        "'self'",
                        'data:',
                        'blob:',
                        'https://market-assets.strapi.io',
                        'https://zucchinibag.blob.core.windows.net',
                    ],

                    upgradeInsecureRequests: null,
                },
            },
        },
    },

    'strapi::cors',
    'strapi::poweredBy',
    'strapi::query',
    'strapi::body',
    'strapi::session',
    'strapi::favicon',
    'strapi::public',
];

export default config;