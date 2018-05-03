# Webhook Notifications

Optimus Cloud can send notifications from instruments to many destinations including services that support Webhooks.

## Supported Formats
* Slack
* Microsoft Teams
* Discord
* Custom

If you would like us to add support for another service please post an issue here or contact support via email.

## How to setup a webhook

### Slack

1. Go to the [Incoming WebHooks](https://my.slack.com/services/new/incoming-webhook/) page in Slack.
2. Choose an existing channel or create a new one.
3. Click **Add Incoming WebHooks Integration**.
4. Copy the **Webhook URL**.
5. Go to the [Webhooks](https://optimus-cloud.co.uk/notifications-webhook) page in Optimus Cloud.
6. Paste in the **URL**, choose **Slack** and click **Add**.

### Microsoft Teams

1. Go to the [Teams](https://teams.microsoft.com) site.
2. Choose the channel you want to use and click **More options (⋯)** next to the name, then Click **Connectors**.
3. Find **Incoming Webhook** from the list and click **Add**.
4. Enter a name for the webhook and click **Create**.
5. Copy the **Webhook URL**.
6. Click **Done**.
7. Go to the [Webhooks](https://optimus-cloud.co.uk/notifications-webhook) page in Optimus Cloud.
8. Paste in the **URL**, choose **Teams** and click **Add**.

### Discord

1. Open Discord.
2. Go to **Server Settings** and select the **Webhooks** tab.
3. Click **Create Webhook**.
4. Choose the channel where messages will be posted and set the name.
5. Copy the **Webhook URL**.
6. Click **Done**.
7. Go to the [Webhooks](https://optimus-cloud.co.uk/notifications-webhook) page in Optimus Cloud.
8. Paste in the **URL**, choose **Discord** and click **Add**.


## Custom Format

If you are making your own webhook service or using it with a service not listed above you can use the custom format.

### Basics

When you recieve a webhook request from Optimus Cloud it includes the message in JSON format as shown below.

```json
{
  "text": "Webhook test message from Optimus Cloud",
  "icon": "https://optimus-cloud.co.uk/images/webhook-icon.png",
  "name": "Optimus Cloud",
  "raw": null
}
```

### Advanced

The **text** property includes a preformatted message representing the notification as you would see in an email or SMS message.

If you want to access more detailed information about the notification the **raw** property contains an object representing the notification information as it came from the instrument.

```json
{
  "text": "G123456: Noise Alert '85dB 1s' 03/05/2018 11:37:13",
  "icon": "https://optimus-cloud.co.uk/images/webhook-icon.png",
  "name": "Optimus Cloud",
  "raw": {
    "Time": "2018-05-03T11:37:13",
    "Name": "85dB 1s"
  }
}
```

The actual contents depend on the type of trigger and instrument used but it will contain at least the Time, Name and Instrument.

*The **raw** property is not available when using the Test button on the website*

If you have any questions or problems post an issue here or contact support via email.