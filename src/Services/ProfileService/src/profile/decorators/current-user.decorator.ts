import { createParamDecorator, ExecutionContext } from '@nestjs/common';
import type { Request } from 'express';

export const CurrentUser = createParamDecorator(
	(data: string | undefined, ctx: ExecutionContext) => {
		const req = ctx.switchToHttp().getRequest<Request>();
		const user = (req as any).user || null;
		if (!user) return null;
		if (!data) return user;
		return user[data];
	},
);