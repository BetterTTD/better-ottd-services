FROM alpine AS builder

ARG OPENGFX_VERSION="7.1"
ARG OPENTTD_VERSION="13.0"

RUN mkdir -p /config \
    && mkdir /tmp/src

RUN apk update && \
    apk add --no-cache --virtual=.buildreqs \
    alpine-sdk \
    wget lzo-dev \
    freetype-dev \
    ca-certificates \
    gcc git xz-dev \
    make cmake xz-libs\
    patch zlib-dev \
    && cd /tmp/src \
    && git clone --verbose --progress https://github.com/OpenTTD/OpenTTD.git . \
    && git fetch --tags \
    && git checkout ${OPENTTD_VERSION} \
    && mkdir /tmp/build && cd /tmp/build \
    && cmake --log-level=VERBOSE \
    -DOPTION_DEDICATED=ON \
    -DOPTION_INSTALL_FHS=OFF \
    -DCMAKE_BUILD_TYPE=release \
    -DGLOBAL_DIR=/app \
    -DPERSONAL_DIR=/ \
    -DCMAKE_BINARY_DIR=bin \
    -DCMAKE_INSTALL_PREFIX=/app \
    ../src \
    && make CMAKE_BUILD_TYPE=release --no-silent -j"$(nproc)" \
    && make --no-silent --trace install \
    && mkdir -p /app/data/baseset \
    && cd /app/data/baseset \
    && rm -rf /tmp/src* \
    && wget --show-progress -q https://cdn.openttd.org/opengfx-releases/${OPENGFX_VERSION}/opengfx-${OPENGFX_VERSION}-all.zip \
    && unzip opengfx-${OPENGFX_VERSION}-all.zip \
    && tar -xf opengfx-${OPENGFX_VERSION}.tar \
    && rm -rf opengfx-*.tar opengfx-*.zip \
    && cd / \
    && apk del .buildreqs

###

FROM alpine

ARG OPENTTD_VERSION="12.2"

LABEL maintainer="tgsamef <tg.same.f@gmail.com>"

WORKDIR /config

RUN mkdir -p /config && \
    adduser -D -h /config -u 1000 -s /bin/false openttd \
    && apk update && \
    apk add --no-cache \
    lzo freetype xz-dev \
    icu-libs zlib vtk xz-libs

COPY --from=builder /app /app

COPY Services/OpenTTD/openttd.cfg /config/openttd.cfg

RUN chown -R openttd:openttd /config /app

VOLUME /config

EXPOSE 3982/tcp

EXPOSE 3982/udp

EXPOSE 3977/tcp

USER openttd

CMD [ "./../app/openttd", "-D", "-c", "openttd.cfg" ]