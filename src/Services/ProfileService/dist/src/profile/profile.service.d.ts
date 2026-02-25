import { PrismaService } from '@/prisma/prisma.service';
import { UpdateUserDto } from './dto/update-user.dto';
import { MeDto } from '@/profile/dto/me.dto';
export declare class ProfileService {
    private readonly prismaService;
    constructor(prismaService: PrismaService);
    getMe(tokenPayload: any): Promise<MeDto>;
    findById(id: string): Promise<{
        id: string;
        email: string;
        name: string;
    }>;
    findByEmail(email: string): Promise<{
        id: string;
        email: string;
        name: string;
    } | null>;
    create(keycloakId: string, email: string, username: string, name: string, surname: string, picture: string): Promise<boolean>;
    update(userId: string, dto: UpdateUserDto): Promise<boolean>;
}
