from surprise import SVD, Dataset, Reader
from surprise.model_selection import train_test_split
from pandas import DataFrame

from .models import UserBehavior

class SVDModel:
    def __init__(self):
        self.model = None

svd = SVDModel()

def update_svd_model(app):
    global svd
    with app.app_context():
        app.logger.info("Starting model update")
        app.logger.info("Loading user behavior data")
        behavior_data, dataset_size = download_user_behavior_data()
        if (dataset_size < 5):
            app.logger.warn(f"Dataset is too small to start training, [dataset_size]={dataset_size}")
            return
        trainset, _ = train_test_split(behavior_data)
        new_svd = SVD()
        app.logger.info("Training...")
        new_svd.fit(trainset)
        svd.model = new_svd
        app.logger.info("Model updated")

def download_user_behavior_data():
    behavior_data = UserBehavior.query.with_entities(
        UserBehavior.user_id,
        UserBehavior.target_user_id,
        UserBehavior.rating
    ).all()
    behavior_df = DataFrame(behavior_data, columns=['user_id', 'target_user_id', 'rating'])
    dataset_size = len(behavior_df.index)
    reader = Reader(rating_scale=(1, 5))
    return Dataset.load_from_df(behavior_df, reader), dataset_size
    