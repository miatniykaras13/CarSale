import { BadRequestException, Injectable, NotFoundException } from '@nestjs/common'

import { PrismaService } from '@/prisma/prisma.service'

import { UpdateProfileDto } from './dto/update-profile.dto'
import { MeDto } from '@/profile/dto/me.dto'
import { KeycloakAdminService } from '@/keycloak/keycloak-admin.service'

@Injectable()
export class ProfileService {
	public constructor(
		private readonly prismaService: PrismaService,
		private readonly keycloakAdminService: KeycloakAdminService,
		) {}

	public async getMe(tokenPayload: any): Promise<MeDto> {
		if (!tokenPayload) {
			throw new BadRequestException('No token provided')
		}

		const id: string = tokenPayload.sub
		const username: string = tokenPayload.preferred_username
		const email: string = tokenPayload.email
		const name: string = tokenPayload.given_name
		const surname: string = tokenPayload.family_name

		// const realmRoles: string[] = tokenPayload?.realm_access?.roles ?? []
		// const resourceRoles: string[] = Object.values(tokenPayload?.resource_access ?? {})
		// 	.flatMap((r: any) => r.roles ?? [])
		// const roles = Array.from(new Set([...realmRoles, ...resourceRoles]))

		const result: MeDto = {
			id,
			username,
			email,
			name,
			surname,
		}

		const user = await this.prismaService.user.findFirst({
				where: {
					keycloakId: id
				}
			}
		)
		
		if(!user)
			await this.create(
				result.id,
				result.email,
				result.username,
				result.name,
				result.surname
			)
        
		return result
	}
	
	public async findById(id: string) {
		const user = await this.prismaService.user.findUnique({
			where: {
				id
			},
			select: {
				email: true,
				name: true,
				id: true
			}
		})

		if (!user) {
			throw new NotFoundException(
				'Пользователь не найден. Пожалуйста, проверьте введенные данные.'
			)
		}

		return user
	}

	public async findByEmail(email: string) {
		const user = await this.prismaService.user.findUnique({
			where: {
				email
			},
			select: {
				email: true,
				name: true,
				id: true
			}
		})

		return user
	}

	public async create(
		keycloakId: string,
		email: string,
		username: string,
		name: string,
		surname: string,
		picture?: string
	) {
		// const user = 
		await this.prismaService.user.create({
			data: {
				keycloakId,
				email,
				username,
				name,
				surname,
				picture
			}
		})

		return true
	}

	public async update(userId: string, dto: UpdateProfileDto) {
		await this.prismaService.user.update({
			where: {
				keycloakId: userId
			},
			data: {
				email: dto.email,
				name: dto.name,
				surname: dto.surname
			}
		})
		
		const keycloakPayload = {
			firstName: dto.name,
			lastName: dto.surname,
			email: dto.email
		}
		
		await this.keycloakAdminService.updateUser(userId, keycloakPayload)

		return true
	}
	
	public async batchProfiles(
		ids: string[]
	) {
		return this.prismaService.user.findMany({
			where: {
				id: {
					in: ids
				}
			}
		})
	}
	
	public async findByUsername(username: string) {
		return this.prismaService.user.findUnique({
			where: {
				username: username
			}
		})
	}
	
	public async deleteUser(id: string) {
		const user = await this.prismaService.user.findUnique({
			where: {
				keycloakId: id
			}
		})
		
		if (!user)
			throw new BadRequestException("User not found")
		
		const result = await this.keycloakAdminService.deleteUser(user.keycloakId)
		
		if(!result.success)
			throw new BadRequestException("Error deleting user")
		
		return this.prismaService.user.delete({
			where: {
				keycloakId: id
			}
		})
	}
	
	public async exists(id: string) {
		const user = await this.prismaService.user.findUnique({
			where: {
				keycloakId: id
			}
		})
		
		return !!user
	}
	
	public async searchByUsername(username: string, userId: any) {
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
		})
		
		return users
	}
    
    public async getShortById(id: string) {
        const user = await this.prismaService.user.findUnique({
            where: {
                id: id
            },
            select: {
                id: true,
                username: true,
                picture: true
            }
        })
        
        if (!user)
            throw new BadRequestException("User not found")
        
        return user
    }
}