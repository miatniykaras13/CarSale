import { ProfileService } from './profile.service';
export declare class ProfileController {
    private readonly profileService;
    constructor(profileService: ProfileService);
    getProfileById(id: string): Promise<{
        name: string;
        email: string;
        id: string;
    }>;
}
