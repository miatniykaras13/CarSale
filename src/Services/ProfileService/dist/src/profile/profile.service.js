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
let ProfileService = class ProfileService {
    prismaService;
    constructor(prismaService) {
        this.prismaService = prismaService;
    }
    async getMe(tokenPayload) {
        if (!tokenPayload) {
            throw new common_1.BadRequestException('No token provided');
        }
        const id = tokenPayload.sub;
        const username = tokenPayload.preferred_username;
        const email = tokenPayload.email;
        const name = tokenPayload.name;
        const surname = tokenPayload.surname;
        const realmRoles = tokenPayload?.realm_access?.roles ?? [];
        const resourceRoles = Object.values(tokenPayload?.resource_access ?? {})
            .flatMap((r) => r.roles ?? []);
        const roles = Array.from(new Set([...realmRoles, ...resourceRoles]));
        const result = {
            id,
            username,
            email,
            name,
            surname,
            roles,
        };
        this.create(result.id, result.email, result.username, result.name, result.surname);
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
        const user = await this.findById(userId);
        await this.prismaService.user.update({
            where: {
                id: user.id
            },
            data: {
                email: dto.email,
                username: dto.username,
                name: dto.name,
                surname: dto.surname
            }
        });
        return true;
    }
};
exports.ProfileService = ProfileService;
exports.ProfileService = ProfileService = __decorate([
    (0, common_1.Injectable)(),
    __metadata("design:paramtypes", [prisma_service_1.PrismaService])
], ProfileService);
//# sourceMappingURL=profile.service.js.map