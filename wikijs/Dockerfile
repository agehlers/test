FROM requarks/wiki:latest

# Replace with your email address:
ENV WIKI_ADMIN_EMAIL admin@example.com

WORKDIR /var/wiki

RUN chown 1001 /var/wiki; chmod -R 777 /var/wiki 

# Replace your-config.yml with the path to your config file:
ADD config.yaml config.yml

EXPOSE 3000
USER 1001

ENTRYPOINT [ "node", "server" ]
