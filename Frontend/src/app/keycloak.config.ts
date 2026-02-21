
import { KeycloakConfig } from 'keycloak-js';

const keycloakConfig: KeycloakConfig = {
  url: 'http://localhost:8080', 
  realm: 'MyAppRealm',
  clientId: 'my-angular-app'
};

export default keycloakConfig;
