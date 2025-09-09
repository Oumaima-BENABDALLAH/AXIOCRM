// src/app/keycloak.config.ts
import { KeycloakConfig } from 'keycloak-js';

const keycloakConfig: KeycloakConfig = {
  url: 'http://localhost:8080', // ton serveur Keycloak
  realm: 'MyAppRealm',
  clientId: 'my-angular-app'
};

export default keycloakConfig;
