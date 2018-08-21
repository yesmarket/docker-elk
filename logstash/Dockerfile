ARG ELK_VERSION

# https://github.com/elastic/logstash-docker
FROM docker.elastic.co/logstash/logstash-oss:${ELK_VERSION}

ENV http_proxy='http://proxy.adelaide.certegy.com:8082'
ENV https_proxy='https://proxy.adelaide.certegy.com:8082'

# Add your logstash plugins setup here
# Example: RUN logstash-plugin install logstash-filter-json
RUN logstash-plugin install logstash-filter-alter
RUN logstash-plugin install logstash-filter-prune
