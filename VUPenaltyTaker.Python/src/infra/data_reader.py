import glob
import json
from pathlib import Path
from typing import List, Dict

import pandas as pd

from src.domain import Direction, TrialData, TrialDataExperiment3And4, Condition


class ParticipantMapper:

    def __init__(self):
        self.participant_mapping: Dict[str, int] = {}
        self.participant_id = 0

    def get_participant_id(self, participant_name: str) -> int:
        if participant_name not in self.participant_mapping:
            self.participant_id += 1
            self.participant_mapping[participant_name] = self.participant_id
        return self.participant_mapping[participant_name]


class Reader:
    @staticmethod
    def find_files(path: Path) -> List[Path]:
        filenames = glob.glob(str(path), recursive=True)
        return [Path(filename) for filename in filenames]

    @classmethod
    def read_json_files(cls, path: Path) -> List[TrialData]:
        mapper = ParticipantMapper()

        filenames = cls.find_files(path)
        data = []

        for filename in filenames:
            with open(filename) as f:
                json_string = f.read()
                json_data = json.loads(json_string)

                participant_id: int = mapper.get_participant_id(json_data['ParticipantName'])
                trial_number: int = json_data['TrialNumber']
                goalkeeper_displacement: float = json_data['GoalkeeperDisplacement']
                goalkeeper_movement_initiation: float = json_data['GoalkeeperStartBeforeKick']

                jump_direction: Direction = Direction.from_string(json_data.get('JumpDirection'))
                advertisement_direction: Direction = Direction.from_string(json_data.get('AdvertisementDirection'))

                shot_location_x: float = json_data['Events']['End']['EndLocation']['X']
                shot_direction: Direction = Direction.LEFT if shot_location_x < 0 else Direction.RIGHT

                data.append(
                    TrialData(
                        participant_id,
                        trial_number,
                        goalkeeper_displacement,
                        shot_location_x,
                        goalkeeper_movement_initiation,
                        shot_direction,
                        jump_direction,
                        advertisement_direction
                    )
                )

        return data

    @classmethod
    def read_csv_files(cls, path: Path) -> List[TrialDataExperiment3And4]:
        filenames = cls.find_files(path)
        data = []

        participant_id = -1
        for filename in filenames:
            participant_id += 1
            with open(filename) as f:
                df = pd.read_csv(f)

                for index, row in df.iterrows():
                    data.append(
                        TrialDataExperiment3And4(
                            participant_id,
                            int(index),
                            Direction.from_string(row.iloc[1]),  # Use iloc to access by position
                            float(row.iloc[0]),  # Use iloc to access by position
                            Condition.from_string(row.iloc[2]),  # Use iloc to access by position
                        )
                    )

        return data
