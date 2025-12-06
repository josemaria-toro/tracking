CREATE SCHEMA IF NOT EXISTS tracking AUTHORIZATION postgres;

CREATE TABLE IF NOT EXISTS tracking.dependencies (
    "app_id" uuid NOT NULL,
    "duration" double precision NOT NULL,
    "id" uuid NOT NULL,
    "input_data" character varying(8192) NOT NULL,
    "name" character varying(128) NOT NULL,
    "operation_id" uuid NOT NULL,
    "output_data" character varying(8192) NOT NULL,
    "success" boolean NOT NULL,
    "target_name" character varying(4096) NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    "type" character varying(128) NOT NULL,
    CONSTRAINT pk_dependencies PRIMARY KEY (app_id, id)
) TABLESPACE pg_default;

ALTER TABLE tracking.dependencies OWNER TO postgres;

CREATE TABLE IF NOT EXISTS tracking.errors (
    "app_id" uuid NOT NULL,
    "error_type_name" character varying(128) NOT NULL,
    "id" uuid NOT NULL,
    "message" character varying(8192) NOT NULL,
    "operation_id" uuid NOT NULL,
    "severity_level" integer NOT NULL,
    "source_type_name" character varying(128) NOT NULL,
    "stack_trace" character varying(8192) NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT pk_errors PRIMARY KEY (app_id, id)
) TABLESPACE pg_default;

ALTER TABLE tracking.errors OWNER TO postgres;

CREATE TABLE IF NOT EXISTS tracking.events (
    "app_id" uuid NOT NULL,
    "id" uuid NOT NULL,
    "metadata" character varying(8192) NOT NULL,
    "name" character varying(128) NOT NULL,
    "operation_id" uuid NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT pk_events PRIMARY KEY (app_id, id)
) TABLESPACE pg_default;

ALTER TABLE tracking.events OWNER TO postgres;

CREATE TABLE IF NOT EXISTS tracking.http_requests (
    "app_id" uuid NOT NULL,
    "body" character varying(8192) NOT NULL,
    "duration" double precision NOT NULL,
    "id" uuid NOT NULL,
    "ip_address" character varying(15) NOT NULL,
    "name" character varying(128) NOT NULL,
    "operation_id" uuid NOT NULL,
    "response_body" character varying(8192) NOT NULL,
    "response_code" integer NOT NULL,
    "success" boolean NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    url character varying(4096) NOT NULL,
    CONSTRAINT pk_http_requests PRIMARY KEY (app_id, id)
) TABLESPACE pg_default;

ALTER TABLE tracking.http_requests OWNER TO postgres;

CREATE TABLE IF NOT EXISTS tracking.metrics (
    "app_id" uuid NOT NULL,
    "dimension_name" character varying(128) NOT NULL,
    "id" uuid NOT NULL,
    "name" character varying(128) NOT NULL,
    "operation_id" uuid NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    "value" double precision NOT NULL,
    CONSTRAINT pk_metrics PRIMARY KEY (app_id, id)
) TABLESPACE pg_default;

ALTER TABLE tracking.metrics OWNER TO postgres;

CREATE TABLE IF NOT EXISTS tracking.page_views (
    "app_id" uuid NOT NULL,
    "device_name" character varying(128) NOT NULL,
    "duration" double precision NOT NULL,
    "id" uuid NOT NULL,
    "ip_address" character varying(15) NOT NULL,
    "name" character varying(128) NOT NULL,
    "operation_id" uuid NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    "url" character varying(4096) NOT NULL,
    "user_agent" character varying(256) NOT NULL,
    CONSTRAINT pk_page_views PRIMARY KEY (app_id, id)
) TABLESPACE pg_default;

ALTER TABLE tracking.page_views OWNER TO postgres;

CREATE TABLE IF NOT EXISTS tracking.tests_results (
    "app_id" uuid NOT NULL,
    "duration" double precision NOT NULL,
    "id" uuid NOT NULL,
    "message" character varying(4096) NOT NULL,
    "name" character varying(128) NOT NULL,
    "operation_id" uuid NOT NULL,
    "success" boolean NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT pk_tests_results PRIMARY KEY (app_id, id)
) TABLESPACE pg_default;

ALTER TABLE tracking.tests_results OWNER TO postgres;

CREATE TABLE IF NOT EXISTS tracking.traces (
    "app_id" uuid NOT NULL,
    "id" uuid NOT NULL,
    "message" character varying(8192) NOT NULL,
    "operation_id" uuid NOT NULL,
    "severity_level" integer NOT NULL,
    "source_type_name" character varying(128) NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT pk_traces PRIMARY KEY (app_id, id)
) TABLESPACE pg_default;

ALTER TABLE tracking.traces OWNER TO postgres;

