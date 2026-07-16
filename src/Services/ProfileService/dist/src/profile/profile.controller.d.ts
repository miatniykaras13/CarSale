import type { Request } from 'express';
import { ProfileService } from './profile.service';
import { MeDto } from '@/profile/dto/me.dto';
import { UpdateProfileDto } from '@/profile/dto/update-profile.dto';
import { KeycloakAdminService } from '@/keycloak/keycloak-admin.service';
export declare class ProfileController {
    private readonly profileService;
    private readonly keycloakAdminService;
    constructor(profileService: ProfileService, keycloakAdminService: KeycloakAdminService);
    getMe(tokenPayload: any): Promise<MeDto>;
    updateProfile(dto: UpdateProfileDto, tokenPayload: any): Promise<boolean>;
    batchProfiles(ids: string[]): Promise<{
        id: string;
        keycloakId: string;
        email: string;
        username: string;
        name: string;
        surname: string;
        picture: string | null;
        createdAt: Date;
        updatedAt: Date;
    }[]>;
    findByUsername(username: string): Promise<{
        id: string;
        keycloakId: string;
        email: string;
        username: string;
        name: string;
        surname: string;
        picture: string | null;
        createdAt: Date;
        updatedAt: Date;
    } | null>;
    deleteMe(tokenPayload: any): Promise<void>;
    exists(id: string): Promise<boolean>;
    search(tokenPayload: any, username: string): Promise<{
        id: string;
        username: string;
        picture: string | null;
    }[]>;
    short(id: string): Promise<{
        id: string;
        username: string;
        picture: string | null;
    }>;
    getProfileById(id: string, req: Request): Promise<{
        id: string;
        email: string;
        name: string;
    }>;
}
