FROM registry.access.redhat.com/rhel7/rhel-tools

ENV SUMMARY="RHEL Tools" \
    DESCRIPTION="RHEL Tools"
 
LABEL summary="$SUMMARY" \
      description="$DESCRIPTION" \
      io.k8s.description="$DESCRIPTION" \
      io.k8s.display-name="Drupal-8.4" \
      io.openshift.tags="builder,rhel" \
      release="1"

COPY runcmd.sh /tmp/
RUN chmod +x /tmp/runcmd.sh

USER 0

CMD ["/tmp/runcmd.sh"]
