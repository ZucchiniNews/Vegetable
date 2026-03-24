import type { Core } from "@strapi/strapi";

const config = ({ env }) => ({
  upload: {
    config: {
      provider: "strapi-provider-upload-azure-sa",
      providerOptions: {
        account: env("AZURE_ACCOUNT_NAME"),
        accountKey: env("AZURE_ACCOUNT_KEY"),
        containerName: env("AZURE_CONTAINER_NAME"),
        serviceBaseURL: env("AZURE_SERVICE_BASE_URL"),
      },
      // THIS disables auto-generated sizes
      actionOptions: {
        upload: {},
        uploadStream: {},
        delete: {},
      },

      // Disable formats
      breakpoints: {},
    },
  },
});

export default config;
