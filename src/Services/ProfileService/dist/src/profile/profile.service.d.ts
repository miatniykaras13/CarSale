import { PrismaService } from '@/prisma/prisma.service';
import { UpdateProfileDto } from './dto/update-profile.dto';
import { MeDto } from '@/profile/dto/me.dto';
import { KeycloakAdminService } from '@/keycloak/keycloak-admin.service';
export declare class ProfileService {
    private readonly prismaService;
    private readonly keycloakAdminService;
    constructor(prismaService: PrismaService, keycloakAdminService: KeycloakAdminService);
    getMe(tokenPayload: any): Promise<MeDto>;
    findById(id: string): Promise<{
        email: string;
        name: string;
        id: string;
    }>;
    findByEmail(email: string): Promise<{
        email: string;
        name: string;
        id: string;
    } | null>;
    create(keycloakId: string, email: string, username: string, name: string, surname: string, picture?: string): Promise<boolean>;
    update(userId: string, dto: UpdateProfileDto): Promise<boolean>;
    batchProfiles(ids: string[]): Promise<{
        email: string;
        username: string;
        name: string;
        surname: string;
        id: string;
        keycloakId: string;
        picture: string | null;
        createdAt: Date;
        updatedAt: Date;
    }[]>;
    findByUsername(username: string): Promise<{
        email: string;
        username: string;
        name: string;
        surname: string;
        id: string;
        keycloakId: string;
        picture: string | null;
        createdAt: Date;
        updatedAt: Date;
    } | null>;
    deleteUser(id: string): Promise<{
        email: string;
        username: string;
        name: string;
        surname: string;
        id: string;
        keycloakId: string;
        picture: string | null;
        createdAt: Date;
        updatedAt: Date;
    }>;
    exists(id: string): Promise<boolean>;
    searchByUsername(username: string, userId: any): Promise<{
        username: string;
        id: string;
        picture: string | null;
    }[]>;
    getShortById(id: string): Promise<{
        username: string;
        id: string;
        picture: string | null;
    }>;
}
