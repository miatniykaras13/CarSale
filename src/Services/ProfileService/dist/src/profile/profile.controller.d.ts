import { ProfileService } from './profile.service';
export declare class ProfileController {
    private readonly profileService;
    constructor(profileService: ProfileService);
    getProfileById(id: string): Promise<{
        id: string;
        email: string;
        name: string;
    }>;
}
