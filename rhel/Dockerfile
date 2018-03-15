FROM registry.access.redhat.com/rhel

ENV SUMMARY="RHEL Tools" \
    DESCRIPTION="RHEL Tools"
 
LABEL summary="$SUMMARY" \
      description="$DESCRIPTION" \
      io.k8s.description="$DESCRIPTION" \
      io.k8s.display-name="$SUMMARY" \
      io.openshift.tags="builder,rhel" \
      release="1"

RUN yum repolist > /dev/null && \
    yum install -y yum-utils && \
    yum-config-manager --disable \* &> /dev/null && \
    yum-config-manager --enable rhel-server-rhscl-7-rpms && \
    yum-config-manager --enable rhel-7-server-rpms && \
    yum-config-manager --enable rhel-7-server-optional-rpms && \
    yum-config-manager --enable rhel-7-server-fastrack-rpms && \
    INSTALL_PKGS="telnet rsh rsync wget curl traceroute net-tools lsof iproute tcpflow" && \
    yum install -y --setopt=tsflags=nodocs $INSTALL_PKGS && \
    rpm -V $INSTALL_PKGS && \
    yum clean all -y
    
COPY runcmd.sh /tmp/
RUN chmod +x /tmp/runcmd.sh

USER 1001

CMD ["/tmp/runcmd.sh"]
