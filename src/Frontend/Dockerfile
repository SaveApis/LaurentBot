FROM node:lts-alpine3.23 AS install
WORKDIR /app/install
COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile --non-interactive --production=false

FROM node:lts-alpine3.23 AS build
WORKDIR /app/build
COPY --from=install /app/install/node_modules ./node_modules
COPY . .
RUN yarn build

FROM node:lts-alpine3.23 AS prod-install
WORKDIR /app
COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile --non-interactive --production=true

FROM node:lts-alpine3.23 AS production
WORKDIR /app
COPY --from=build /app/build/dist ./dist
COPY --from=prod-install /app/node_modules ./node_modules
EXPOSE 8080
CMD ["npx", "http-server", "dist", "-p", "8080", "-c-1"]
