FROM node:20



WORKDIR /app



COPY package.json pnpm-lock.yaml* ./



RUN corepack enable

RUN corepack prepare pnpm@9.12.3 --activate



COPY . .



RUN pnpm install --frozen-lockfile



ARG PROFILESERVICE_POSTGRES_URL

ENV PROFILESERVICE_POSTGRES_URL=$PROFILESERVICE_POSTGRES_URL



RUN npx prisma generate



RUN pnpm build



RUN pnpm prune --prod



RUN chmod +x docker-entrypoint.sh



EXPOSE 4000



ENTRYPOINT ["./docker-entrypoint.sh"]


