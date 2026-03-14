FROM node:20

WORKDIR /app

COPY package.json pnpm-lock.yaml* ./

RUN apt-get update && apt-get install -y npm

RUN npm install -g pnpm@latest

COPY . .

RUN pnpm install --frozen-lockfile

ARG PROFILESERVICE_POSTGRES_URL
ENV PROFILESERVICE_POSTGRES_URL=$PROFILESERVICE_POSTGRES_URL

RUN npx prisma generate

RUN pnpm build

RUN pnpm prune --prod

EXPOSE 6003

CMD ["node", "dist/src/main.js"]