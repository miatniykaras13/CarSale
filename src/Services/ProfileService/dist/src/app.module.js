"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.AppModule = void 0;
const common_1 = require("@nestjs/common");
const app_controller_1 = require("./app.controller");
const app_service_1 = require("./app.service");
const profile_module_1 = require("./profile/profile.module");
const ad_module_1 = require("./ad/ad.module");
const config_1 = require("@nestjs/config");
const nest_keycloak_connect_1 = require("nest-keycloak-connect");
const core_1 = require("@nestjs/core");
const config_module_1 = require("./config/config.module");
const keycloak_config_service_1 = require("./config/keycloak-config.service");
const axios_1 = require("@nestjs/axios");
let AppModule = class AppModule {
};
exports.AppModule = AppModule;
exports.AppModule = AppModule = __decorate([
    (0, common_1.Module)({
        imports: [
            profile_module_1.ProfileModule,
            ad_module_1.AdModule,
            config_1.ConfigModule.forRoot({
                isGlobal: true
            }),
            nest_keycloak_connect_1.KeycloakConnectModule.registerAsync({
                useClass: keycloak_config_service_1.KeycloakConfigService,
                imports: [config_module_1.ConfigModules, config_1.ConfigModule],
            }),
            axios_1.HttpModule.register({
                timeout: 5000,
                maxRedirects: 5
            }),
            config_1.ConfigModule
        ],
        controllers: [app_controller_1.AppController],
        providers: [
            app_service_1.AppService,
            {
                provide: core_1.APP_GUARD,
                useClass: nest_keycloak_connect_1.AuthGuard
            },
            {
                provide: core_1.APP_GUARD,
                useClass: nest_keycloak_connect_1.ResourceGuard
            },
            {
                provide: core_1.APP_GUARD,
                useClass: nest_keycloak_connect_1.RoleGuard
            }
        ],
    })
], AppModule);
//# sourceMappingURL=app.module.js.map