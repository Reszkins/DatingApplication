FROM python:3.9-slim

RUN apt-get update && apt-get install -y \
    gcc \
    libpq-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY MATCHER/requirements.txt .

RUN pip install --no-cache-dir -r requirements.txt

COPY MATCHER/matcher matcher

ENV FLASK_APP=matcher
ENV FLASK_DEBUG=0

EXPOSE 5000

CMD [ "python3", "-m", "flask", "run", "--host=0.0.0.0" ]
