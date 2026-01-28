CREATE TABLE IF NOT EXISTS device_usage
(
    device_id   UUID        NOT NULL,
    user_name   TEXT        NOT NULL,
    start_time  TIMESTAMPTZ NOT NULL,
    end_time    TIMESTAMPTZ NOT NULL,
    app_version TEXT        NOT NULL
);

CREATE INDEX IF NOT EXISTS ix_device_usage_device_id
    ON device_usage (device_id);

