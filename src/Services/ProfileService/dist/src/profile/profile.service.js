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
Object.defineProperty(exports, "__esModule", { value: true });
exports.ProfileService = void 0;
const common_1 = require("@nestjs/common");
const prisma_service_1 = require("../prisma/prisma.service");
const keycloak_admin_service_1 = require("../keycloak/keycloak-admin.service");
let ProfileService = class ProfileService {
    prismaService;
    keycloakAdminService;
    constructor(prismaService, keycloakAdminService) {
        this.prismaService = prismaService;
        this.keycloakAdminService = keycloakAdminService;
    }
    async getMe(tokenPayload) {
        if (!tokenPayload) {
            throw new common_1.BadRequestException('No token provided');
        }
        const id = tokenPayload.sub;
        const username = tokenPayload.preferred_username;
        const email = tokenPayload.email;
        const name = tokenPayload.given_name;
        const surname = tokenPayload.family_name;
        const result = {
            id,
            username,
            email,
            name,
            surname,
        };
        const user = await this.prismaService.user.findFirst({
            where: {
                keycloakId: id
            }
        });
        if (!user)
            await this.create(result.id, result.email, result.username, result.name, result.surname);
        return result;
    }
    async findById(id) {
        const user = await this.prismaService.user.findUnique({
            where: {
                id
            },
            select: {
                email: true,
                name: true,
                id: true
            }
        });
        if (!user) {
            throw new common_1.NotFoundException('Пользователь не найден. Пожалуйста, проверьте введенные данные.');
        }
        return user;
    }
    async findByEmail(email) {
        const user = await this.prismaService.user.findUnique({
            where: {
                email
            },
            select: {
                email: true,
                name: true,
                id: true
            }
        });
        return user;
    }
    async create(keycloakId, email, username, name, surname, picture) {
        await this.prismaService.user.create({
            data: {
                keycloakId,
                email,
                username,
                name,
                surname,
                picture
            }
        });
        return true;
    }
    async update(userId, dto) {
        await this.prismaService.user.update({
            where: {
                keycloakId: userId
            },
            data: {
                email: dto.email,
                name: dto.name,
                surname: dto.surname
            }
        });
        const keycloakPayload = {
            firstName: dto.name,
            lastName: dto.surname,
            email: dto.email
        };
        await this.keycloakAdminService.updateUser(userId, keycloakPayload);
        return true;
    }
    async batchProfiles(ids) {
        return this.prismaService.user.findMany({
            where: {
                id: {
                    in: ids
                }
            }
        });
    }
    async findByUsername(username) {
        return this.prismaService.user.findUnique({
            where: {
                username: username
            }
        });
    }
    async deleteUser(id) {
        const user = await this.prismaService.user.findUnique({
            where: {
                keycloakId: id
            }
        });
        if (!user)
            throw new common_1.BadRequestException("User not found");
        const result = await this.keycloakAdminService.deleteUser(user.keycloakId);
        if (!result.success)
            throw new common_1.BadRequestException("Error deleting user");
        return this.prismaService.user.delete({
            where: {
                keycloakId: id
            }
        });
    }
    async exists(id) {
        const user = await this.prismaService.user.findUnique({
            where: {
                keycloakId: id
            }
        });
        return !!user;
    }
    async searchByUsername(username, userId) {
        const users = await this.prismaService.user.findMany({
            where: {
                username: {
                    contains: username,
                    mode: 'insensitive'
                },
                keycloakId: {
                    not: userId
                }
            },
            select: {
                id: true,
                username: true,
                picture: true
            },
            take: 10
        });
        return users;
    }
    async getShortById(id) {
        const user = await this.prismaService.user.findUnique({
            where: {
                id: id
            },
            select: {
                id: true,
                username: true,
                picture: true
            }
        });
        if (!user)
            throw new common_1.BadRequestException("User not found");
        return user;
    }
};
exports.ProfileService = ProfileService;
exports.ProfileService = ProfileService = __decorate([
    (0, common_1.Injectable)(),
    __metadata("design:paramtypes", [prisma_service_1.PrismaService,
        keycloak_admin_service_1.KeycloakAdminService])
], ProfileService);
//# sourceMappingURL=profile.service.js.map