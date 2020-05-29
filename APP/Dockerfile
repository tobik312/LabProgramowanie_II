#Build reactJS-app
FROM mhart/alpine-node:12 AS builder
WORKDIR /app
COPY . ./
RUN npm install --silent
RUN npm run build

#Run app on port 80
FROM mhart/alpine-node:12
RUN yarn global add serve
WORKDIR /app
COPY --from=builder /app/build .
CMD ["serve", "-p", "80", "-s", "."]