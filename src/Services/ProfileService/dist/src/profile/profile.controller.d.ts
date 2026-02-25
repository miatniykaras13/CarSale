import type { Request } from 'express';
import { ProfileService } from './profile.service';
import { MeDto } from '@/profile/dto/me.dto';
export declare class ProfileController {
    private readonly profileService;
    constructor(profileService: ProfileService);
    getMe(tokenPayload: any): Promise<MeDto>;
    getProfileById(id: string, req: Request): Promise<{
        id: string;
        email: string;
        name: string;
    }>;
}
