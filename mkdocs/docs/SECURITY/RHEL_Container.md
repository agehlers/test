# How Containers are Secured on RHEL

Because OpenShift Container Platform runs on Red Hat Enterprise Linux (RHEL) and RHEL Atomic Host, the following concepts apply by default to any deployed OpenShift Container Platform cluster and are at the core of what make containers secure on the platform.

* Linux namespaces enable creating an abstraction of a particular global system resource to make it appear as a separate instance to processes within a namespace. Consequently, several containers can use the same resource simultaneously without creating a conflict. See Overview of Containers in Red Hat Systems for details on the types of namespaces (e.g., mount, PID, and network).
* SELinux provides an additional layer of security to keep containers isolated from each other and from the host. SELinux allows administrators to enforce mandatory access controls (MAC) for every user, application, process, and file.
* CGroups (control groups) limit, account for, and isolate the resource usage (CPU, memory, disk I/O, network, etc.) of a collection of processes. CGroups are used to ensure that containers on the same host are not impacted by each other.
* Secure computing mode (seccomp) profiles can be associated with a container to restrict available system calls.
* Deploying containers using RHEL Atomic Host reduces the attack surface by minimizing the host environment and tuning it for containers. 

Reference: https://access.redhat.com/documentation/en-us/openshift_container_platform/3.7/html/container_security_guide/

