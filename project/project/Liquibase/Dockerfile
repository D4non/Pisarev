FROM liquibase/liquibase:latest

USER root
RUN wget -q https://jdbc.postgresql.org/download/postgresql-42.7.3.jar -O /liquibase/lib/postgresql.jar && \
    chmod 644 /liquibase/lib/postgresql.jar
USER liquibase

WORKDIR /liquibase/changelog

COPY master.xml .
COPY change-sets/ ./change-sets/

CMD ["--changeLogFile=master.xml", "--url=jdbc:postgresql://db:5432/test_db", "--username=test_db", "--password=test_db", "update"]

