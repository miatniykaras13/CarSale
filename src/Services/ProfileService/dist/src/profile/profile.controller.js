"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.ProfileController = void 0;
const common_1 = require("@nestjs/common");
const profile_service_1 = require("./profile.service");
const nest_keycloak_connect_1 = require("nest-keycloak-connect");
const current_user_decorator_1 = require("./decorators/current-user.decorator");
const update_profile_dto_1 = require("./dto/update-profile.dto");
const keycloak_admin_service_1 = require("../keycloak/keycloak-admin.service");
let ProfileController = class ProfileController {
    profileService;
    keycloakAdminService;
    constructor(profileService, keycloakAdminService) {
        this.profileService = profileService;
        this.keycloakAdminService = keycloakAdminService;
    }
    async getMe(tokenPayload) {
        return await this.profileService.getMe(tokenPayload);
    }
    async updateProfile(dto, tokenPayload) {
        return this.profileService.update(tokenPayload.sub, dto);
    }
    async batchProfiles(ids) {
        return await this.profileService.batchProfiles(ids);
    }
    async findByUsername(username) {
        return await this.profileService.findByUsername(username);
    }
    async deleteMe(tokenPayload) {
        await this.profileService.deleteUser(tokenPayload.sub);
    }
    async exists(id) {
        return await this.profileService.exists(id);
    }
    async search(tokenPayload, username) {
        return this.profileService.searchByUsername(username, tokenPayload.sub);
    }
    async short(id) {
        return this.profileService.getShortById(id);
    }
    async getProfileById(id, req) {
        return await this.profileService.findById(id);
    }
};
exports.ProfileController = ProfileController;
__decorate([
    (0, common_1.Get)('/me'),
    __param(0, (0, current_user_decorator_1.CurrentUser)()),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "getMe", null);
__decorate([
    (0, common_1.Put)('/me'),
    __param(0, (0, common_1.Body)()),
    __param(1, (0, current_user_decorator_1.CurrentUser)()),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [update_profile_dto_1.UpdateProfileDto, Object]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "updateProfile", null);
__decorate([
    (0, common_1.Post)('/batch'),
    __param(0, (0, common_1.Body)()),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Array]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "batchProfiles", null);
__decorate([
    (0, common_1.Get)(),
    __param(0, (0, common_1.Query)('username')),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [String]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "findByUsername", null);
__decorate([
    (0, common_1.Delete)('/me'),
    (0, common_1.HttpCode)(204),
    __param(0, (0, current_user_decorator_1.CurrentUser)()),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "deleteMe", null);
__decorate([
    (0, common_1.Get)('exists/:id'),
    __param(0, (0, common_1.Param)('id')),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [String]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "exists", null);
__decorate([
    (0, common_1.Get)('/search'),
    __param(0, (0, current_user_decorator_1.CurrentUser)()),
    __param(1, (0, common_1.Query)('username')),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, String]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "search", null);
__decorate([
    (0, nest_keycloak_connect_1.Public)(),
    (0, common_1.Get)('/:id/short'),
    __param(0, (0, common_1.Param)('id')),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [String]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "short", null);
__decorate([
    (0, common_1.Get)('/:id'),
    __param(0, (0, common_1.Param)('id')),
    __param(1, (0, common_1.Req)()),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [String, Object]),
    __metadata("design:returntype", Promise)
], ProfileController.prototype, "getProfileById", null);
exports.ProfileController = ProfileController = __decorate([
    (0, common_1.Controller)('profiles'),
    __metadata("design:paramtypes", [profile_service_1.ProfileService,
        keycloak_admin_service_1.KeycloakAdminService])
], ProfileController);
//# sourceMappingURL=profile.controller.js.map