CREATE TABLE `events` (
    `id` BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
    `datetime` DATETIME NOT NULL,
    `event_type` VARCHAR(30) NOT NULL COLLATE 'utf8mb4_unicode_ci',
    `mediashare_type` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `alertplaying_type` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `from_user` TEXT NOT NULL COLLATE 'utf8mb4_unicode_ci',
    `message` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `amount` DECIMAL(10,2) NULL DEFAULT NULL,
    `currency` VARCHAR(4) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `months` INT(4) UNSIGNED NULL DEFAULT NULL,
    `subscription_plan` VARCHAR(10) NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `raiders` INT(10) UNSIGNED NULL DEFAULT NULL,
    `media_url` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `media_id` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `media_title` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `media_views` BIGINT(20) UNSIGNED NULL DEFAULT NULL,
    `media_start_time` INT(6) UNSIGNED NULL DEFAULT NULL,
    `media_channel_url` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `media_channel_id` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `media_channel_title` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `media_thumbnail_url` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_unicode_ci',
    `raw_json` LONGTEXT NOT NULL COLLATE 'utf8mb4_unicode_ci',
    PRIMARY KEY (`id`),
    INDEX `datetime` (`datetime`),
    INDEX `event_type` (`event_type`),
    FULLTEXT INDEX `from_user` (`from_user`)
)
COLLATE='utf8mb4_unicode_ci'
ENGINE=InnoDB
;
