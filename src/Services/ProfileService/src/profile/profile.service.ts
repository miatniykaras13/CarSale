 import { BadRequestException, Injectable, NotFoundException } from '@nestjs/common'

import { PrismaService } from '@/prisma/prisma.service'

import { UpdateProfileDto } from './dto/update-profile.dto'
 import { MeDto } from '@/profile/dto/me.dto'

@Injectable()
export class ProfileService {

	public constructor(private readonly prismaService: PrismaService) {}

	public async getMe(tokenPayload: any): Promise<MeDto> {
		if (!tokenPayload) {
			throw new BadRequestException('No token provided')
		}

		const id: string = tokenPayload.sub
		const username: string = tokenPayload.preferred_username
		const email: string = tokenPayload.email
		const name: string = tokenPayload.name
		const surname: string = tokenPayload.surname

		const realmRoles: string[] = tokenPayload?.realm_access?.roles ?? []
		const resourceRoles: string[] = Object.values(tokenPayload?.resource_access ?? {})
			.flatMap((r: any) => r.roles ?? [])
		const roles = Array.from(new Set([...realmRoles, ...resourceRoles]))

		const result: MeDto = {
			id,
			username,
			email,
			name,
			surname,
			roles,
		}

        this.create(
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
		const user = await this.findById(userId)

		// const updatedUser = 
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
		})

		return true
	}
}