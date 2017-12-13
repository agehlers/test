FROM registry.access.redhat.com/rhscl/php-71-rhel7 php-test-app

ENV SUMMARY="Drupal 8.4 with Apache 2.4 and PHP 7.1." \
    DESCRIPTION="Drupal 8.4, Apache 2.4, PHP 7.1"
 
LABEL summary="$SUMMARY" \
      description="$DESCRIPTION" \
      io.k8s.description="$DESCRIPTION" \
      io.k8s.display-name="Drupal-8.4" \
      io.openshift.expose-services="8080:http" \
      io.openshift.tags="builder,drupal8,drupal" \
      release="1"
 
# NOTES:
# We need to call 2 (!) yum commands before being able to enable repositories properly
# This is a workaround for https://bugzilla.redhat.com/show_bug.cgi?id=1479388
# Chrome install info: https://access.redhat.com/discussions/917293
RUN yum repolist > /dev/null && \
    yum install -y yum-utils && \
    yum-config-manager --disable \* &> /dev/null && \
    yum-config-manager --enable rhel-server-rhscl-7-rpms && \
    yum-config-manager --enable rhel-7-server-rpms && \
    yum-config-manager --enable rhel-7-server-optional-rpms && \
    yum-config-manager --enable rhel-7-server-fastrack-rpms && \
    INSTALL_PKGS="tar wget gzip mod_ssl rh-php71-php-fpm" && \
    yum install -y --setopt=tsflags=nodocs $INSTALL_PKGS && \
    rpm -V $INSTALL_PKGS && \
    yum clean all -y && \
    wget -c https://ftp.drupal.org/files/projects/drupal-8.4.3.tar.gz && \
    gunzip drupal-8.4.3.tar.gz &&\
    tar -xf drupal-8.4.3.tar &&\
    if [ ! -d /opt/app-root ]; then mkdir /opt/app-root; fi &&\
    if [ ! -d /opt/app-root/src ]; then mkdir /opt/app-root/src; fi &&\
    mv drupal-8.4.3 /opt/app-root/src &&\
    #cp /opt/app-root/src/drupal/sites/default.settings.php /opt/app-root/src/drupal/sites/settings.php &&\
    #chown -R apache:apache /opt/apt-root/src/drupal/ &&\
    #chcon -R -t httpd_sys_content_rw_t /opt/app-root/src/drupal/sites/ &&\
    rm drupal-8.4.3.tar

#RUN easy_install supervisor
# Still need drush installed
#RUN pear channel-discover pear.drush.org && pear install drush/drush

# Add a drushrc file to point to default site
#ADD drushrc.php /etc/drush/drushrc.php

# Install registry rebuild tool.  This is helpful when your Drupal registry gets 
## broken from moving modules around
#RUN drush @none dl registry_rebuild --nocolor

# we run Drupal with a memory_limit of 512M
#RUN sed -i "s/memory_limit = 128M/memory_limit = 512M/" /etc/php.ini
# we run Drupal with an increased file size upload limit as well
#RUN sed -i "s/upload_max_filesize = 2M/upload_max_filesize = 100M/" /etc/php.ini
#RUN sed -i "s/post_max_size = 8M/post_max_size = 100M/" /etc/php.ini

USER 1001
