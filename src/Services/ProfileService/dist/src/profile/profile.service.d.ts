import { PrismaService } from '@/prisma/prisma.service';
import { UpdateUserDto } from './dto/update-user.dto';
export declare class ProfileService {
    private readonly prismaService;
    constructor(prismaService: PrismaService);
    findById(id: string): Promise<{
        name: string;
        email: string;
        id: string;
    }>;
    findByEmail(email: string): Promise<{
        name: string;
        email: string;
        id: string;
    } | null>;
    create(keycloakId: string, email: string, name: string, picture: string): Promise<boolean>;
    update(userId: string, dto: UpdateUserDto): Promise<boolean>;
}
