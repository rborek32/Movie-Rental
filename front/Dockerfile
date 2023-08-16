FROM node:lts-alpine

RUN npm install -g http-server

WORKDIR /app
COPY package*.json ./

RUN npm install --force

COPY . .
RUN npm run build

EXPOSE 9002
CMD [ "http-server", "dist" ]
