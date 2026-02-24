import type { Request } from 'express';
import { ProfileService } from './profile.service';
export declare class ProfileController {
    private readonly profileService;
    constructor(profileService: ProfileService);
    getProfileById(id: string, req: Request): Promise<{
        name: string;
        email: string;
        id: string;
    }>;
}
