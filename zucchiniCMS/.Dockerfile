
FROM node:18-alpine

WORKDIR /opt/app

COPY . .

RUN npm install
RUN npm build

EXPOSE 1337

CMD ["npm", "start"]